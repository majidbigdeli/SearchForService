using System;
using SearchForApi.Models.Entities;

namespace SearchForApi.Factories
{
    public interface IHistoryFactory
    {
        History CreateNewHistoryInstance(Guid? userId, HistoryType type, HistoryReferType referType, string keyword, bool found, SceneLangaugeType language, int? hitIndex = null, int? hitCount = null, Guid? sceneId = null);
        bool HistoryIsValid(History history);
    }
}