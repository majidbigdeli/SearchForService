using System;
using System.Threading.Tasks;
using Nest;
using SearchForApi.Models.Entities;

namespace SearchForApi.Services
{
    public interface ILookupService
    {
        Task<(HistoryType type, SceneLangaugeType language, string cleanedPhrase, ISearchResponse<ElasticSubtitleEntity> result)> Lookup(Guid userId, string phrase, int skip);
        Task<(HistoryType type, SceneLangaugeType language, string cleanedPhrase, ISearchResponse<ElasticSubtitleEntity> result)> LookupPredefined(Guid? userId, string phrase, int skip);
    }
}
