using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MethodTimer;
using SearchForApi.Core;
using SearchForApi.Factories;
using SearchForApi.Models.Entities;
using SearchForApi.Repositories;

namespace SearchForApi.Services
{
    public class HistoryService : IHistoryService
    {
        private readonly HistoryRepository _historyRepository;
        private readonly IHistoryFactory _historyFactory;
        private readonly IUserService _userService;
        private readonly IDateTimeFactory _dateTimeFactory;

        public HistoryService(HistoryRepository historyRepository, IHistoryFactory historyFactory, IUserService userService, IDateTimeFactory dateTimeFactory)
        {
            _historyRepository = historyRepository;
            _historyFactory = historyFactory;
            _userService = userService;
            _dateTimeFactory = dateTimeFactory;
        }

        [Time("userId={userId},type={type},keyword={keyword},found={found},language={language},sceneId={sceneId}")]
        public async Task<History> AddNewSearchHistory(Guid? userId, HistoryType type, HistoryReferType referType, string keyword, bool found, SceneLangaugeType language, int? hitIndex = null, int? hitCount = null, Guid? sceneId = null)
        {
            var newHistory = _historyFactory.CreateNewHistoryInstance(userId, type,referType, keyword, found, language, hitIndex, hitCount, sceneId);
            await _historyRepository.Insert(newHistory);

            return newHistory;
        }

        [Time("userId={userId},skip={skip},take={take}")]
        public async Task<(long total, List<History>)> GetUserSearchHistories(Guid userId, int skip, int take)
        {
            var userPlan = await _userService.GetUserCurrentPlanStatus(userId);
            var startDate = _dateTimeFactory.UtcNow.AddDays(userPlan.LastHistoriesDays * -1);

            var result = await _historyRepository.GetUserSearchHistories(userId, startDate, skip, take);
            return result;
        }

        [Time]
        public async Task<Dictionary<string, string>> GetSuggestions(Guid? userId)
        {
            var result = await _historyRepository.GetKeywordsRandomly(Cfg.MaxSearchSeggestionItemCount);

            return result;
        }
    }
}