using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MethodTimer;
using SearchForApi.Factories;
using SearchForApi.Models;
using SearchForApi.Models.Entities;
using SearchForApi.Models.Exceptions;
using SearchForApi.Repositories;

namespace SearchForApi.Services
{
    public class SearchService : ISearchService
    {
        private readonly ISceneService _sceneService;
        private readonly IHistoryService _historyService;
        private readonly IPlanService _planService;
        private readonly IMapper _mapper;
        private readonly SceneRepository _sceneRepository;
        private readonly ILinkFactory _linkFactory;
        private readonly ILookupService _lookupService;
        private readonly HistoryRepository _historyRepository;
        private readonly IHistoryFactory _historyFactory;

        public SearchService(ISceneService sceneService, IHistoryService historyService, IPlanService planService, IMapper mapper, SceneRepository sceneRepository, ILinkFactory linkFactory, ILookupService lookupService, HistoryRepository historyRepository, IHistoryFactory historyFactory)
        {
            _sceneService = sceneService;
            _historyService = historyService;
            _planService = planService;
            _mapper = mapper;
            _sceneRepository = sceneRepository;
            _linkFactory = linkFactory;
            _lookupService = lookupService;
            _historyRepository = historyRepository;
            _historyFactory = historyFactory;
        }

        [Time("userId={userId},phrase={phrase},skip={skip}")]
        public async Task<SearchResultModel> Search(Guid userId, string phrase, int skip)
        {
            var (type, language, cleanedPhrase, result) = await _lookupService.Lookup(userId, phrase, skip);

            var firstHitItem = result.Hits.First();
            var firstItem = await _sceneService.GetScene(userId, firstHitItem.Source);

            await _historyService.AddNewSearchHistory(userId, type, HistoryReferType.Manual, cleanedPhrase, true, language, skip, (int)result.Total, Guid.Parse(firstItem.Id));

            // note: the status update must be checked for both search and scenes to ensure the user doesn't search unlimited scenes with different keywords programmatically.
            await _planService.UpdateUserStatus(userId, UserPlanStatusType.Search, false, cleanedPhrase);

            return new SearchResultModel
            {
                Total = result.Total,
                Hit = firstItem,
            };
        }

        [Time("userId={userId},sceneId={sceneId}")]
        public async Task<SerarchResultItemModel> GetSceneItem(Guid userId, Guid sceneId)
        {
            var existScene = await _sceneRepository.GetWithMovie(sceneId);
            if (existScene == null)
                throw new ValidationException();

            if (existScene.CheckResultType == SceneCheckResultType.Excluded)
                throw new ValidationException();

            var sceneItem = await _sceneService.GetScene(userId, _mapper.Map<ElasticSubtitleEntity>(existScene));

            return sceneItem;
        }

        [Time("sharedToken={sharedToken}")]
        public async Task<SharedResultItemModel> GetSharedItem(string sharedToken)
        {
            var existItem = await _sceneService.UpdateSharedHistory(sharedToken);

            var sceneItem = await _sceneService.GetScene(existItem.UserId, _mapper.Map<ElasticSubtitleEntity>(existItem.Scene), false);

            var normalizedResult = _mapper.Map<SharedResultItemModel>(sceneItem, opt =>
            {
                opt.AfterMap((src, dest) =>
                {
                    dest.Keyword = existItem.Keyword;
                    dest.ImageUrl = _linkFactory.GetSceneAssetImageUrl(existItem.Scene.Movie, SceneAssetType.Cover);
                });
            });

            return normalizedResult;
        }

        [Time("itemId={itemId},userId={userId},skip={skip}")]
        public async Task<SearchResultModel> GetHistoryScenes(Guid itemId, Guid? userId, int skip)
        {
            var existItem = await _historyRepository.Get(itemId);
            if (!_historyFactory.HistoryIsValid(existItem))
                throw new ValidationException();

            var (type, language, cleanedPhrase, result) = await _lookupService.LookupPredefined(userId, existItem.SearchKeyword, skip);

            var firstHitItem = result.Hits.First();
            var firstItem = await _sceneService.GetScene(userId, firstHitItem.Source);

            await _historyService.AddNewSearchHistory(userId, type, HistoryReferType.History, cleanedPhrase, true, language, skip, (int)result.Total, Guid.Parse(firstItem.Id));

            return new SearchResultModel
            {
                Total = result.Total,
                Hit = firstItem,
            };
        }
    }
}

