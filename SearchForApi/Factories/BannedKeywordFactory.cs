using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MethodTimer;
using Microsoft.Extensions.Caching.Memory;
using SearchForApi.Models.Entities;
using SearchForApi.Repositories;
using SearchForApi.Utilities.LockManager;

namespace SearchForApi.Factories
{
    public class BannedKeywordFactory : IBannedKeywordFactory
    {
        private readonly BannedKeywordRepository _bannedKeywordRepository;
        private readonly IMemoryCache _memoryCache;

        private static readonly string bannedKeywordPattern = "(^|\\s|[\\.\\\\\"\\?\\'\\-_])(?<keyword>{0})(\\s|$|[\\.\\\\\"\\?\\'\\-_])";
        private static readonly string bannedKeywordCacheKey = "BANNED_KEYWORDS";

        public BannedKeywordFactory(BannedKeywordRepository bannedKeywordRepository, IMemoryCache memoryCache)
        {
            _bannedKeywordRepository = bannedKeywordRepository;
            _memoryCache = memoryCache;
        }

        [Time("language={language}")]
        private async Task<List<string>> getBannedKeywords(SceneLangaugeType language)
        {
            var languageBannedKeywordCacheKey = $"{bannedKeywordCacheKey}_{language}";

            _memoryCache.TryGetValue(languageBannedKeywordCacheKey, out List<string> bannedKeywords);

            using (var lockManager = new GetBannedKeywordsLockManager(bannedKeywords != null))
            {
                await lockManager.AcquireLock(languageBannedKeywordCacheKey);

                _memoryCache.TryGetValue(languageBannedKeywordCacheKey, out bannedKeywords);
                if (bannedKeywords == null)
                {
                    bannedKeywords = (await _bannedKeywordRepository.GetByLanguage(language)).Select(p => p.Keyword).ToList();
                    _memoryCache.Set(languageBannedKeywordCacheKey, bannedKeywords, new MemoryCacheEntryOptions()
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
                    });
                }

                return bannedKeywords;
            }
        }

        [Time("language={language}")]
        public async Task<bool> HasBannedKeyword(string text, SceneLangaugeType language)
        {
            var bannedKeywords = await getBannedKeywords(language);
            foreach (var bannedKeyword in bannedKeywords)
            {
                var pattern = string.Format(bannedKeywordPattern, Regex.Escape(bannedKeyword));
                var found = Regex.IsMatch(text, pattern, RegexOptions.IgnoreCase);

                if (found) return true;
            }

            return false;
        }

        [Time("language={language}")]
        public async Task<string> MaskBannedKeyword(string text, SceneLangaugeType language)
        {
            var bannedKeywords = await getBannedKeywords(language);
            foreach (var bannedKeyword in bannedKeywords)
            {
                var pattern = string.Format(bannedKeywordPattern, Regex.Escape(bannedKeyword));
                var mask = '*';
                text = Regex.Replace(text, pattern, m => m.Groups[1].Value + new string(mask, m.Groups["keyword"].Length) + m.Groups[2].Value, RegexOptions.IgnoreCase);
            }

            return text;
        }
    }

    public class GetBannedKeywordsLockManager : LockManager<string>
    {
        public GetBannedKeywordsLockManager(bool ignore) : base("GetBannedKeywordsLockManager", ignore) { }
    }
}