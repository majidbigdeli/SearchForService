using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SearchForApi.Factories;
using SearchForApi.Models;
using SearchForApi.Models.Dtos;
using SearchForApi.Models.Entities;
using SearchForApi.Models.Exceptions;
using SearchForApi.Repositories;
using SearchForApi.Services;
using SearchForApi.Utilities;

namespace SearchForApi.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("feature")]
    [ApiVersion("2.0")]
    public class FeatureController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly FeatureRepository _featureRepository;
        private readonly ILinkFactory _linkFactory;
        private readonly FeatureItemRepository _featureItemRepository;
        private readonly IFeatureService _featureService;

        public FeatureController(IMapper mapper, FeatureRepository featureRepository, ILinkFactory linkFactory, FeatureItemRepository featureItemRepository, IFeatureService featureService)
        {
            _mapper = mapper;
            _featureRepository = featureRepository;
            _linkFactory = linkFactory;
            _featureItemRepository = featureItemRepository;
            _featureService = featureService;
        }

        [HttpGet]
        public async Task<ListResponseDto<FeatureDto>> Get()
        {
            if (!ModelState.IsValid)
                throw new ValidationException();

            var result = await _featureRepository.GetAvailable();

            return new ListResponseDto<FeatureDto>(result.Select(item => _mapper.Map<Feature, FeatureDto>(item, opt =>
                opt.AfterMap((src, dest) =>
                {
                    if (src.Cover != null)
                        dest.CoverUrl = _linkFactory.GetFeatureCoverUrl(src.Cover);
                }))
            ).ToList());
        }

        [HttpGet("{id}")]
        public async Task<ResponseDto<FeatureDto>> GetById(Guid id)
        {
            if (!ModelState.IsValid)
                throw new ValidationException();

            var result = await _featureService.GetById(id);

            return new ResponseDto<FeatureDto>(_mapper.Map<Feature, FeatureDto>(result, opt =>
                opt.AfterMap((src, dest) =>
                {
                    if (src.Cover != null)
                        dest.CoverUrl = _linkFactory.GetFeatureCoverUrl(src.Cover);
                }))
            );
        }

        [HttpGet("{id}/items/{skip:int}/{take:int}")]
        public async Task<PaginationResponseDto<FeatureItemDto>> GetItems(Guid id, int skip = 0, int take = 10)
        {
            if (!ModelState.IsValid)
                throw new ValidationException();

            var (total, items) = await _featureService.GetItems(id, User.IdOrDefault(), skip, take);

            return new PaginationResponseDto<FeatureItemDto>(total, items.Select(item => _mapper.Map<FeatureItem, FeatureItemDto>(item, opt =>
                opt.AfterMap((src, dest) =>
                {
                    if (src.Cover != null)
                        dest.CoverUrl = _linkFactory.GetFeatureCoverUrl(src.Cover);
                }))
            ).ToList());
        }

        [HttpGet("item/{id}/scene/{skip:int}")]
        public async Task<ResponseDto<SearchResultDto>> GetItemScenes(Guid id, int skip)
        {
            if (!ModelState.IsValid)
                throw new ValidationException();

            var result = await _featureService.GetItemScenes(id, User.IdOrDefault(), skip);
            return new ResponseDto<SearchResultDto>(_mapper.Map<SearchResultDto>(result));
        }

        [HttpGet("item/{id}/scene")]
        public async Task<ResponseDto<SerarchResultItemModel>> GetItemScene(Guid id)
        {
            if (!ModelState.IsValid)
                throw new ValidationException();

            var result = await _featureService.GetItemScene(id, User.IdOrDefault());
            return new ResponseDto<SerarchResultItemModel>(result);
        }
    }
}

