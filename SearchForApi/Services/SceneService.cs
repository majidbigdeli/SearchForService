using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MethodTimer;
using Nest;
using SearchForApi.Factories;
using SearchForApi.Models;
using SearchForApi.Models.Entities;
using SearchForApi.Models.Exceptions;
using SearchForApi.Repositories;

namespace SearchForApi.Services
{
    public class SceneService : ISceneService
    {
        private readonly MovieRepository _movieRepository;
        private readonly SceneFileRepository _sceneFileRepository;
        private readonly ILinkFactory _linkFactory;
        private readonly ISceneFactory _sceneFactory;
        private readonly SceneRepository _sceneRepository;
        private readonly BookmarkRepository _bookmarkRepository;
        private readonly ReportRepository _reportRepository;
        private readonly IPlanService _planService;
        private readonly IShareFactory _shareFactory;
        private readonly ShareRepository _shareRepository;
        private readonly IShareHistoryFactory _shareHistoryFactory;
        private readonly ShareHistoryRepository _shareHistoryRepository;


        public SceneService(MovieRepository movieRepository, SceneFileRepository sceneFileRepository, ILinkFactory linkFactory, ISceneFactory sceneFactory, SceneRepository sceneRepository, BookmarkRepository bookmarkRepository, ReportRepository reportRepository, IPlanService planService, IShareFactory shareFactory, ShareRepository shareRepository, IShareHistoryFactory shareHistoryFactory, ShareHistoryRepository shareHistoryRepository)
        {
            _movieRepository = movieRepository;
            _sceneFileRepository = sceneFileRepository;
            _linkFactory = linkFactory;
            _sceneFactory = sceneFactory;
            _sceneRepository = sceneRepository;
            _bookmarkRepository = bookmarkRepository;
            _reportRepository = reportRepository;
            _planService = planService;
            _shareFactory = shareFactory;
            _shareRepository = shareRepository;
            _shareHistoryFactory = shareHistoryFactory;
            _shareHistoryRepository = shareHistoryRepository;
        }

        [Time("userId={userId}")]
        public async Task<SerarchResultItemModel> GetScene(Guid? userId, ElasticSubtitleEntity subtitle, bool withUserItems = true)
        {
            var nearItems = await _sceneRepository.GetCheckedNearItems(Guid.Parse(subtitle.MovieId), subtitle.StartTime, subtitle.EndTime);

            var segmentInfo = _sceneFactory.CalculateSceneTimes(subtitle.StartTime, subtitle.EndTime);

            var texts = nearItems
                .Where(p => p.Language == SceneLangaugeType.En)
                .Select(p => _sceneFactory.CreateNewResultItemTextItem(p, segmentInfo.ActualStartSegmentTime));
            var translations = nearItems
                .Where(p => p.Language == SceneLangaugeType.Fa)
                .Select(p => _sceneFactory.CreateNewResultItemTextItem(p, segmentInfo.ActualStartSegmentTime));

            var isUserBookemarkedScene = userId != null && withUserItems && await _bookmarkRepository.IsUserBookemarkedScene((Guid)userId, Guid.Parse(subtitle.SceneId));

            return new SerarchResultItemModel
            {
                Id = subtitle.SceneId,
                Texts = await Task.WhenAll(texts),
                SceneUrl = _linkFactory.GetStreamUrl(userId, subtitle.MovieId, (int)subtitle.StartTime, (int)subtitle.EndTime),
                Translations = await Task.WhenAll(translations),
                MovieName = subtitle.MovieName,
                IsBookmarked = isUserBookemarkedScene
            };
        }

        [Time("id={id},originStartTime={originStartTime},originEndTime={originEndTime}")]
        public async Task<Stream> GenerateScenePlayListFile(string id, int originStartTime, int originEndTime)
        {
            if (string.IsNullOrEmpty(id))
                throw new ValidationException();

            var movie = await _movieRepository.Get(Guid.Parse(id));
            if (movie == null)
                throw new ItemNotFoundException();

            var segmentInfo = _sceneFactory.CalculateSceneTimes(originStartTime, originEndTime);

            var result = new List<string>() {
                "#EXTM3U",
                "#EXT-X-VERSION:3",
                "#EXT-X-PLAYLIST-TYPE:VOD",
                "#EXT-X-TARGETDURATION:4",
                $"#EXT-X-START:TIME-OFFSET={segmentInfo.StartTime/1000.0},PRECISE=YES"
            };

            for (int i = segmentInfo.StartSegment; i <= segmentInfo.EndSegment; i++)
            {
                var segmentS3Url = _sceneFileRepository.GeneratePreSignedURL(movie.BucketName, $"{movie.StorageId}_0_0_{i}.ts");

                result.Add("#EXTINF:4.000000");
                result.Add(_linkFactory.GetSegmentUrl(movie.BucketName, segmentS3Url));
            }

            result.Add("#EXT-X-ENDLIST");

            var fileContent = string.Join('\n', result);
            return new MemoryStream(Encoding.UTF8.GetBytes(fileContent));
        }

        [Time("userId={userId},sceneId={sceneId}")]
        public async Task<UserBookmarkModel> Bookmark(Guid userId, Guid sceneId)
        {
            await _planService.IsAllowedToBookmark(userId);

            var existScene = await _sceneRepository.GetWithMovie(sceneId);
            if (existScene == null)
                throw new ValidationException();

            var isUserBookemarkedScene = await _bookmarkRepository.IsUserBookemarkedScene(userId, sceneId);
            if (isUserBookemarkedScene)
                throw new ValidationException();

            var newItem = _sceneFactory.CreateNewBookmarkInstance(userId, existScene);
            await _bookmarkRepository.Insert(newItem);

            await _planService.UpdateUserStatus(userId, UserPlanStatusType.Bookmark, false);

            return await _sceneFactory.CreateNewBookmarkModelInstance(newItem);
        }

        [Time("userId={userId},sceneId={sceneId}")]
        public async Task UnBookmark(Guid userId, Guid sceneId)
        {
            var existBookmark = await _bookmarkRepository.GetByUserIdAndSceneId(userId, sceneId);
            if (existBookmark == null)
                throw new ValidationException();

            await _bookmarkRepository.Delete(existBookmark);

            await _planService.UpdateUserStatus(userId, UserPlanStatusType.Bookmark, true);
        }

        [Time("userId={userId},sceneId={sceneId}")]
        public async Task<string> Share(Guid userId, Guid sceneId, string keyword)
        {
            await _planService.IsAllowedToShare(userId);

            var isSceneExist = await _sceneRepository.Exist(sceneId);
            if (!isSceneExist)
                throw new ValidationException();

            var newItem = _shareFactory.CreateNewShareInstance(userId, sceneId, keyword);
            await _shareRepository.Insert(newItem);

            await _planService.UpdateUserStatus(userId, UserPlanStatusType.Share, false);

            return _linkFactory.GetSceneShareUrl(keyword, newItem.Token);
        }

        [Time("sharedToken={sharedToken}")]
        public async Task<Share> UpdateSharedHistory(string sharedToken)
        {
            var existItem = await _shareRepository.GetByTokenWithSceneAndMovie(sharedToken);
            if (existItem == null)
                throw new ItemNotFoundException();

            if (existItem.Scene.CheckResultType == SceneCheckResultType.Excluded)
                throw new ItemNotFoundException();

            _shareFactory.UpdateShareItemView(existItem);

            var newShareHistory = _shareHistoryFactory.CreateNewShareHistoryInstance(existItem.Id);
            _shareHistoryRepository.Add(newShareHistory);

            await _shareRepository.SaveChanges();

            return existItem;
        }

        [Time("userId={userId},sceneId={sceneId},type={type}")]
        public async Task Report(Guid userId, Guid sceneId, ReportType type)
        {
            var isSceneExist = await _sceneRepository.Exist(sceneId);
            if (!isSceneExist)
                throw new ValidationException();

            var newItem = _sceneFactory.CreateNewReportInstance(userId, sceneId, type);
            await _reportRepository.Insert(newItem);
        }
    }
}

