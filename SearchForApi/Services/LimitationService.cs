using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MethodTimer;
using SearchForApi.Factories;
using SearchForApi.Models;
using SearchForApi.Models.Dtos;
using SearchForApi.Models.Entities;
using SearchForApi.Models.Exceptions;
using SearchForApi.Repositories;

namespace SearchForApi.Services
{
    public class LimitationService : ILimitationService
    {
        private readonly MovieRepository _movieRepository;
        private readonly ILinkFactory _linkFactory;
        private readonly IDateTimeFactory _dateTimeFactory;
        private readonly ElasticSubtitleRepository _subtitleRepository;
        private readonly ISceneFactory _sceneFactory;
        private readonly SceneRepository _sceneRepository;
        private readonly ShareRepository _shareRepository;
        private readonly ISceneService _sceneService;
        private readonly IMapper _mapper;
        private readonly HistoryRepository _historyRepository;
        private readonly BookmarkRepository _bookmarkRepository;

        public LimitationService(MovieRepository movieRepository, ILinkFactory linkFactory, IDateTimeFactory dateTimeFactory, ElasticSubtitleRepository subtitleRepository, ISceneFactory sceneFactory, SceneRepository sceneRepository, ShareRepository shareRepository, ISceneService sceneService, IMapper mapper, HistoryRepository historyRepository, BookmarkRepository bookmarkRepository)
        {
            _movieRepository = movieRepository;
            _linkFactory = linkFactory;
            _dateTimeFactory = dateTimeFactory;
            _subtitleRepository = subtitleRepository;
            _sceneFactory = sceneFactory;
            _sceneRepository = sceneRepository;
            _shareRepository = shareRepository;
            _sceneService = sceneService;
            _mapper = mapper;
            _historyRepository = historyRepository;
            _bookmarkRepository = bookmarkRepository;
        }

        [Time]
        public async Task<(long total, MovieAssetDto result)> GetUncheckedMovieAsset()
        {
            var (total, movie) = await _movieRepository.GetAssetsUncheckedMovie();
            if (total == 0) return (0, null);

            var result = new MovieAssetDto
            {
                MovieId = movie.Id,
                MovieName = movie.Title,
                CoverImageUrl = _linkFactory.GetSceneAssetImageUrl(movie, SceneAssetType.Cover),
                ThumbnailImageUrl = _linkFactory.GetSceneAssetImageUrl(movie, SceneAssetType.PosterLarge),
            };
            return (total, result);
        }

        [Time("movieId={movieId},status={status}")]
        public async Task CheckdMovieAsset(Guid userId, Guid movieId, AssetsCheckStatus status)
        {
            var movie = await _movieRepository.Get(movieId);
            if (movie == null)
                throw new ValidationException();

            movie.AssetsCheckedByUserId = userId;
            movie.AssetsCheckedOn = _dateTimeFactory.UtcNow;
            movie.AssetsCheckedStatus = status;

            await _movieRepository.SaveChanges();
        }

        [Time("userId={userId},sceneId={sceneId},type={type},resultType={resultType},excludeType={excludeType}")]
        public async Task Check(Guid userId, Guid sceneId, SceneCheckType type, SceneCheckResultType resultType, SceneCheckExcludeType? excludeType)
        {
            var existItem = await _sceneRepository.Get(sceneId);
            if (existItem == null)
                throw new ValidationException();

            var nearItems = await _sceneRepository.GetNearItems(existItem.MovieId, existItem.StartTime, existItem.EndTime);

            if (resultType == SceneCheckResultType.Normal)
            {
                foreach (var nearItem in nearItems)
                    _sceneFactory.CheckedSceneAsNormal(userId, nearItem, type, sceneId);
            }
            else
            {
                if (excludeType == null)
                    throw new ValidationException();

                var nearItemsIds = nearItems.Select(p => p.Id.ToString()).ToList();
                await _subtitleRepository.ExcludeDocsBySceneId(nearItemsIds);

                foreach (var nearItem in nearItems)
                    _sceneFactory.ExcludeCheckedScene(userId, nearItem, type, (SceneCheckExcludeType)excludeType, sceneId);
            }

            await _sceneRepository.SaveChanges();
        }
    }
}
