using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SearchForApi.Models.Entities;

namespace SearchForApi.Services
{
    public interface IHistoryService
    {
        Task<History> AddNewSearchHistory(Guid? userId, HistoryType type, HistoryReferType referType, string keyword, bool found, SceneLangaugeType language, int? hitIndex = null, int? hitCount = null, Guid? sceneId = null);
        Task<(long total, List<History>)> GetUserSearchHistories(Guid userId, int skip, int take);
        Task<Dictionary<string, string>> GetSuggestions(Guid? userId);
    }
}