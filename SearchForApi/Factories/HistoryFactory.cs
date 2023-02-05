using System;
using SearchForApi.Models.Entities;

namespace SearchForApi.Factories
{
    public class HistoryFactory : IHistoryFactory
    {
        private readonly IDateTimeFactory _dateTimeFactory;

        public HistoryFactory(IDateTimeFactory dateTimeFactory)
        {
            _dateTimeFactory = dateTimeFactory;
        }

        public History CreateNewHistoryInstance(Guid? userId, HistoryType type, HistoryReferType referType, string keyword, bool found, SceneLangaugeType language, int? hitIndex = null, int? hitCount = null, Guid? sceneId = null)
        {
            var newHistory = new History
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CreatedOn = _dateTimeFactory.UtcNow,
                Type = type,
                ReferType = referType,
                SearchIsFound = found,
                SearchKeyword = keyword,
                SceneId = sceneId,
                Language = language,
                HitIndex = hitIndex,
                HitsCount = hitCount
            };

            return newHistory;
        }

        public bool HistoryIsValid(History history)
        {
            return history != null && history.SearchIsFound;
        }
    }
}