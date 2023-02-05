using System;
using System.Threading.Tasks;
using SearchForApi.Models;
using SearchForApi.Models.Entities;

namespace SearchForApi.Factories
{
    public interface ISceneFactory
    {
        CalculateSegmentModel CalculateSceneTimes(int startTime, int endTime);
        Bookmark CreateNewBookmarkInstance(Guid userId, Scene scene);
        Task<UserBookmarkModel> CreateNewBookmarkModelInstance(Bookmark item);
        Report CreateNewReportInstance(Guid userId, Guid sceneId, ReportType type);
        Task<SearchResultItemTextItems> CreateNewResultItemTextItem(Scene scene, int actualStartSegmentTime);
        Scene CheckedSceneAsNormal(Guid userId, Scene scene, SceneCheckType type, Guid checkedBySceneId);
        Scene ExcludeCheckedScene(Guid userId, Scene scene, SceneCheckType type, SceneCheckExcludeType excludeType, Guid checkedBySceneId);
    }
}

