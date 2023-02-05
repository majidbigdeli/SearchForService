using System;
using System.IO;
using System.Threading.Tasks;
using SearchForApi.Models;
using SearchForApi.Models.Entities;

namespace SearchForApi.Services
{
    public interface ISceneService
    {
        Task<SerarchResultItemModel> GetScene(Guid? userId, ElasticSubtitleEntity subtitle, bool withUserItems = true);
        Task<Stream> GenerateScenePlayListFile(string id, int originStartTime, int originEndTime);
        Task<UserBookmarkModel> Bookmark(Guid userId, Guid sceneId);
        Task UnBookmark(Guid userId, Guid sceneId);
        Task<string> Share(Guid userId, Guid sceneId, string keyword);
        Task<Share> UpdateSharedHistory(string sharedToken);
        Task Report(Guid userId, Guid sceneId, ReportType type);
    }
}

