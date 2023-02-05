using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SearchForApi.Factories;
using SearchForApi.Models.Dtos;
using SearchForApi.Repositories;
using SearchForApi.Services;
using SearchForApi.Utilities;

namespace SearchForApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("user/list")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class ListController : ControllerBase
    {
        private readonly IHistoryService _historyService;
        private readonly BookmarkRepository _bookmarkRepository;
        private readonly IMapper _mapper;
        private readonly ISceneFactory _sceneFactory;

        public ListController(IHistoryService historyService, IMapper mapper, BookmarkRepository bookmarkRepository, ISceneFactory sceneFactory)
        {
            _historyService = historyService;
            _mapper = mapper;
            _bookmarkRepository = bookmarkRepository;
            _sceneFactory = sceneFactory;
        }

        private readonly int _take = 10;
        private (int skip, int take) GetPaginationFromPage(int page) => (Math.Max(0, page - 1) * _take, _take);

        [HttpGet("bookmarks/{skip:int}")]
        public async Task<PaginationResponseDto<UserBookmarkDto>> GetBookmarks(int skip)
        {
            var (total, items) = await _bookmarkRepository.GetUserCheckdBookmarks(User.Id(), skip, _take);
            var normalizedTaskItem = items.Select(p => _sceneFactory.CreateNewBookmarkModelInstance(p));

            var result = new PaginationResponseDto<UserBookmarkDto>(
                total,
                _mapper.Map<List<UserBookmarkDto>>(await Task.WhenAll(normalizedTaskItem))
            );

            return result;
        }

        [HttpGet("search-histories/{skip:int}")]
        public async Task<PaginationResponseDto<UserSearchHistoryDto>> GetSearchHistories(int skip)
        {
            var (total, items) = await _historyService.GetUserSearchHistories(User.Id(), skip, _take);

            return new PaginationResponseDto<UserSearchHistoryDto>(
                total,
                _mapper.Map<List<UserSearchHistoryDto>>(items)
            );
        }
    }
}