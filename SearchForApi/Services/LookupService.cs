using System;
using System.Threading.Tasks;
using MethodTimer;
using Nest;
using SearchForApi.Factories;
using SearchForApi.Integrations.Symspell;
using SearchForApi.Models.Entities;
using SearchForApi.Models.Exceptions;
using SearchForApi.Repositories;
using SearchForApi.Utilities;

namespace SearchForApi.Services
{
    public class LookupService : ILookupService
    {
        private readonly IPlanService _planService;
        private readonly IBannedKeywordFactory _bannedKeywordFactory;
        private readonly IUserService _userService;
        private readonly ElasticSubtitleRepository _subtitleRepository;
        private readonly IHistoryService _historyService;
        private readonly ISymspellIntegration _symspellIntegration;

        public LookupService(IPlanService planService, IBannedKeywordFactory bannedKeywordFactory, IUserService userService, ElasticSubtitleRepository subtitleRepository, IHistoryService historyService, ISymspellIntegration symspellIntegration)
        {
            _planService = planService;
            _bannedKeywordFactory = bannedKeywordFactory;
            _userService = userService;
            _subtitleRepository = subtitleRepository;
            _historyService = historyService;
            _symspellIntegration = symspellIntegration;
        }

        [Time("userId={userId},phrase={phrase},skip={skip}")]
        public async Task<(HistoryType type, SceneLangaugeType language, string cleanedPhrase, ISearchResponse<ElasticSubtitleEntity> result)> Lookup(Guid userId, string phrase, int skip)
        {
            phrase = phrase.CleanKeyword();
            var type = skip == 0 ? HistoryType.Search : HistoryType.Scene;

            if (string.IsNullOrEmpty(phrase) || phrase.Length < 3)
                throw new ValidationException();

            var phraseLanguage = phrase.IsPersian() ? SceneLangaugeType.Fa : SceneLangaugeType.En;

            if (phraseLanguage == SceneLangaugeType.En)
                phrase = await _symspellIntegration.CorrectPhrase(phrase);

            await _planService.IsAllowedToSearch(userId, phrase, skip);

            var hasBannedItem = await _bannedKeywordFactory.HasBannedKeyword(phrase, phraseLanguage);
            if (hasBannedItem)
                throw new SearchBannedKeywordException();

            var userPlan = await _userService.GetUserCurrentPlanStatus(userId);
            var result = await _subtitleRepository.LookUpPhrase(phrase, skip, userPlan.KidsMode, phraseLanguage);

            var isFound = result != null && result.Total > 0 && result.Total > skip;
            if (!isFound)
            {
                await _historyService.AddNewSearchHistory(userId, type, HistoryReferType.Manual, phrase, false, phraseLanguage);
                throw new ItemNotFoundException();
            }

            return (type, phraseLanguage, phrase, result);
        }

        [Time("userId={userId},phrase={phrase},skip={skip}")]
        public async Task<(HistoryType type, SceneLangaugeType language, string cleanedPhrase, ISearchResponse<ElasticSubtitleEntity> result)> LookupPredefined(Guid? userId, string phrase, int skip)
        {
            phrase = phrase.CleanKeyword();
            var type = skip == 0 ? HistoryType.Search : HistoryType.Scene;

            await _planService.IsAllowedViewPredefinedItemScene(userId, skip);

            var phraseLanguage = phrase.IsPersian() ? SceneLangaugeType.Fa : SceneLangaugeType.En;

            var result = await _subtitleRepository.LookUpPhrase(phrase, skip, false, phraseLanguage);

            return (type, phraseLanguage, phrase, result);
        }
    }
}
