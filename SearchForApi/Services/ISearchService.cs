using System;
using System.Threading.Tasks;
using SearchForApi.Models;

namespace SearchForApi.Services
{
    public interface ISearchService
    {
        Task<SearchResultModel> Search(Guid userId, string phrase, int skip);
        Task<SerarchResultItemModel> GetSceneItem(Guid userId, Guid sceneId);
        Task<SharedResultItemModel> GetSharedItem(string sharedToken);
        Task<SearchResultModel> GetHistoryScenes(Guid itemId, Guid? userId, int skip);
    }
}

