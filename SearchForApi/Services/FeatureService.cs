using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MethodTimer;
using SearchForApi.Core;
using SearchForApi.Factories;
using SearchForApi.Models;
using SearchForApi.Models.Entities;
using SearchForApi.Models.Exceptions;
using SearchForApi.Repositories;

namespace SearchForApi.Services
{
    public class FeatureService : IFeatureService
    {
        private readonly FeatureRepository _featureRepository;
        private readonly FeatureItemRepository _featureItemRepository;
        private readonly IUserService _userService;
        private readonly ILookupService _lookupService;
        private readonly ISceneService _sceneService;
        private readonly IHistoryService _historyService;
        private readonly IFeatureFactory _featureFactory;
        private readonly IMapper _mapper;
        private readonly SceneRepository _sceneRepository;

        public FeatureService(FeatureRepository featureRepository, FeatureItemRepository featureItemRepository, IUserService userService, ILookupService lookupService, ISceneService sceneService, IHistoryService historyService, IFeatureFactory featureFactory, IMapper mapper, SceneRepository sceneRepository)
        {
            _featureRepository = featureRepository;
            _featureItemRepository = featureItemRepository;
            _userService = userService;
            _lookupService = lookupService;
            _sceneService = sceneService;
            _historyService = historyService;
            _featureFactory = featureFactory;
            _mapper = mapper;
            _sceneRepository = sceneRepository;
        }

        [Time("id={id}")]
        public async Task<Feature> GetById(Guid id)
        {
            var existItem = await _featureRepository.Get(id);
            if (!_featureFactory.FeatureIsValid(existItem))
                throw new ValidationException();

            return existItem;
        }

        [Time("id={id},userId={userId},skip={skip},take={take}")]
        public async Task<(long total, List<FeatureItem>)> GetItems(Guid id, Guid? userId, int skip, int take)
        {
            var normalizedSkip = 0;
            var normalizedTake = Math.Min(Cfg.MaxFeatureItemCount, take);

            if (userId != null)
            {
                var userPlanStatus = await _userService.GetUserCurrentPlanStatus((Guid)userId);
                if (userPlanStatus.Type != PlanType.Basic) normalizedSkip = skip;
            }

            var result = await _featureItemRepository.GetByFeatureId(id, normalizedSkip, normalizedTake);

            return result;
        }

        [Time("itemId={itemId},userId={userId},skip={skip}")]
        public async Task<SearchResultModel> GetItemScenes(Guid itemId, Guid? userId, int skip)
        {
            var existItem = await _featureItemRepository.GetWithFeature(itemId);
            if (!_featureFactory.FeatureItemIsValid(existItem))
                throw new ValidationException();

            if (!_featureFactory.IsSupportOperation(existItem.Feature, FeatureType.Category, FeatureType.DailyWord, FeatureType.List))
                throw new ValidationException();

            var (type, language, cleanedPhrase, result) = await _lookupService.LookupPredefined(userId, existItem.Keyword, skip);

            var firstHitItem = result.Hits.First();
            var firstItem = await _sceneService.GetScene(userId, firstHitItem.Source);

            await _historyService.AddNewSearchHistory(userId, type, HistoryReferType.Feature, cleanedPhrase, true, language, skip, (int)result.Total, Guid.Parse(firstItem.Id));

            return new SearchResultModel
            {
                Total = result.Total,
                Hit = firstItem,
            };
        }

        [Time("itemId={itemId},userId={userId}")]
        public async Task<SerarchResultItemModel> GetItemScene(Guid itemId, Guid? userId)
        {
            var existItem = await _featureItemRepository.GetWithFeature(itemId);
            if (!_featureFactory.FeatureItemIsValid(existItem))
                throw new ValidationException();

            if (!_featureFactory.IsSupportOperation(existItem.Feature, FeatureType.DailyScene))
                throw new ValidationException();

            var existScene = await _sceneRepository.GetWithMovie(Guid.Parse(existItem.ParameterId));
            if (existScene == null)
                throw new ValidationException();

            var sceneItem = await _sceneService.GetScene(userId, _mapper.Map<ElasticSubtitleEntity>(existScene));

            return sceneItem;
        }
    }
}

