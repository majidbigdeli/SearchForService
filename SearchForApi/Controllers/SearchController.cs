using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SearchForApi.Models.Dtos;
using SearchForApi.Models.Exceptions;
using SearchForApi.Services;
using SearchForApi.Utilities;

namespace SearchForApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("search")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;
        private readonly ILookupService _lookupService;
        private readonly IMapper _mapper;
        private readonly IHistoryService _historyService;

        public SearchController(ISearchService searchService, IMapper mapper, ILookupService lookupService, IHistoryService historyService)
        {
            _searchService = searchService;
            _lookupService = lookupService;
            _mapper = mapper;
            _historyService = historyService;
        }

        [HttpGet("{keyword}/{skip:int}")]
        public async Task<ResponseDto<SearchResultDto>> SearchFor(string keyword, int skip)
        {
            if (!ModelState.IsValid)
                throw new ValidationException();

            var result = await _searchService.Search(User.Id(), keyword, skip);
            return new ResponseDto<SearchResultDto>(_mapper.Map<SearchResultDto>(result));
        }

        [HttpGet("{keyword}")]
        public async Task<ResponseDto<Null>> CheckForSearch(string keyword)
        {
            if (!ModelState.IsValid)
                throw new ValidationException();

            await _lookupService.Lookup(User.Id(), keyword, 0);
            return new ResponseDto<Null>();
        }

        [AllowAnonymous]
        [HttpGet("suggestion")]
        [MapToApiVersion("2.0")]
        public async Task<ListResponseDto<SearchSuggestionDto>> GetSuggestions()
        {
            if (!ModelState.IsValid)
                throw new ValidationException();

            var result = await _historyService.GetSuggestions(User.IdOrDefault());
            var normalizedResult = result
                .Select(p => new SearchSuggestionDto { Id = p.Key, Keyword = p.Value })
                .ToList();

            return new ListResponseDto<SearchSuggestionDto>(normalizedResult);
        }

        [AllowAnonymous]
        [HttpGet("history/{id}/scene/{skip:int}")]
        [MapToApiVersion("2.0")]
        public async Task<ResponseDto<SearchResultDto>> GetHistoryScenes(Guid id, int skip)
        {
            if (!ModelState.IsValid)
                throw new ValidationException();

            var result = await _searchService.GetHistoryScenes(id, User.IdOrDefault(), skip);
            return new ResponseDto<SearchResultDto>(_mapper.Map<SearchResultDto>(result));
        }
    }
}

