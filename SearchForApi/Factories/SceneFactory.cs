using System;
using System.Threading.Tasks;
using SearchForApi.Models;
using SearchForApi.Models.Entities;

namespace SearchForApi.Factories
{
    public class SceneFactory : ISceneFactory
    {
        private readonly IDateTimeFactory _dateTimeFactory;
        private readonly IBannedKeywordFactory _bannedKeywordFactory;

        public SceneFactory(IDateTimeFactory dateTimeFactory, IBannedKeywordFactory bannedKeywordFactory)
        {
            _dateTimeFactory = dateTimeFactory;
            _bannedKeywordFactory = bannedKeywordFactory;
        }

        public CalculateSegmentModel CalculateSceneTimes(int startTime, int endTime)
        {
            var segmentSize = 4000;
            var startBreathTime = 1000;
            var endBreathTime = 3000;

            var startSegmentNumber = startTime / segmentSize;
            var endSegmentNumber = endTime / segmentSize;

            var actualStartSegmentTime = startSegmentNumber * segmentSize;
            var actualEndSegmentTime = (endSegmentNumber + 1) * segmentSize;

            var startSegmentDiff = Math.Max(0, startTime - actualStartSegmentTime - startBreathTime);
            var duration = Math.Min(endTime - startTime + endBreathTime, ((endSegmentNumber - startSegmentNumber + 1) * segmentSize) - startSegmentDiff);

            return new CalculateSegmentModel
            {
                StartSegment = startSegmentNumber,
                EndSegment = endSegmentNumber,
                SegmentCount = (endSegmentNumber - startSegmentNumber) + 1,
                StartTime = startSegmentDiff,
                EndTime = startSegmentDiff + duration,
                DurationTime = duration,
                StartBreathTime = startBreathTime,
                EndBreathTime = endBreathTime,
                OriginSurationTime = ((endSegmentNumber - startSegmentNumber) + 1) * segmentSize,
                ActualStartSegmentTime = actualStartSegmentTime + 500,
                ActualEndSegmentTime = actualEndSegmentTime - 500
            };
        }

        public async Task<SearchResultItemTextItems> CreateNewResultItemTextItem(Scene scene, int actualStartSegmentTime)
        {
            var start = scene.StartTime - actualStartSegmentTime;
            var end = start + (scene.EndTime - scene.StartTime);

            return new SearchResultItemTextItems
            {
                Text = await _bannedKeywordFactory.MaskBannedKeyword(scene.PlainText, scene.Language),
                Start = Math.Max(0, start),
                End = end,
            };
        }

        public Bookmark CreateNewBookmarkInstance(Guid userId, Scene scene)
        {
            return new Bookmark
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CreatedOn = _dateTimeFactory.UtcNow,
                Scene = scene
            };
        }

        public async Task<UserBookmarkModel> CreateNewBookmarkModelInstance(Bookmark item)
        {
            return new UserBookmarkModel
            {
                CreatedOn = item.CreatedOn,
                Id = item.SceneId.ToString(),
                Text = await _bannedKeywordFactory.MaskBannedKeyword(item.Scene.NormalizedPlainText, item.Scene.Language),
                MovieName = item.Scene.Movie.Title
            };
        }

        public Report CreateNewReportInstance(Guid userId, Guid sceneId, ReportType type)
        {
            return new Report
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CreatedOn = _dateTimeFactory.UtcNow,
                SceneId = sceneId,
                Type = type,
            };
        }

        public Scene CheckedSceneAsNormal(Guid userId, Scene scene, SceneCheckType type, Guid checkedBySceneId)
        {
            scene.CheckedByUserId = userId;
            scene.CheckedOn = _dateTimeFactory.UtcNow;
            scene.CheckType = type;
            scene.CheckResultType = SceneCheckResultType.Normal;

            if (scene.Id != checkedBySceneId)
                scene.CheckedBySceneId = checkedBySceneId;

            return scene;
        }

        public Scene ExcludeCheckedScene(Guid userId, Scene scene, SceneCheckType type, SceneCheckExcludeType excludeType, Guid checkedBySceneId)
        {
            scene.CheckedByUserId = userId;
            scene.CheckedOn = _dateTimeFactory.UtcNow;
            scene.CheckType = type;
            scene.CheckResultType = SceneCheckResultType.Excluded;
            scene.CheckExcludeType = excludeType;

            if (scene.Id != checkedBySceneId)
                scene.CheckedBySceneId = checkedBySceneId;

            return scene;
        }
    }
}

