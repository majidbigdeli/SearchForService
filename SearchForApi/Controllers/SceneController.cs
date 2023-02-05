using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SearchForApi.Factories;
using SearchForApi.Models;
using SearchForApi.Models.Dtos;
using SearchForApi.Models.Inputs;
using SearchForApi.Services;
using SearchForApi.Utilities;

namespace SearchForApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("scene")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class SceneController : ControllerBase
    {
        private readonly ISceneService _sceneService;
        private readonly ISearchService _searchService;
        private readonly ILinkFactory _linkFactory;
        private readonly IMapper _mapper;

        public SceneController(ISceneService sceneService, ILinkFactory linkFactory, IMapper mapper, ISearchService searchService)
        {
            _sceneService = sceneService;
            _linkFactory = linkFactory;
            _mapper = mapper;
            _searchService = searchService;
        }

        [AllowAnonymous]
        [HttpGet("file/{code}.m3u8")]
        public async Task<IActionResult> GetScenePlayList(string code)
        {
            var (userId, id, start, end) = _linkFactory.ParseStreamUrl(code);

            var stream = await _sceneService.GenerateScenePlayListFile(id, start, end);
            return File(stream, "application/x-mpegURL");
        }

        [HttpGet("{sceneId}")]
        public async Task<ResponseDto<SerarchResultItemModel>> GetScene(Guid sceneId)
        {
            var result = await _searchService.GetSceneItem(User.Id(), sceneId);
            return new ResponseDto<SerarchResultItemModel>(result);
        }

        [HttpPost("share/{sceneId}")]
        public async Task<ResponseDto<string>> ShareScene(Guid sceneId, [FromBody] ShareSceneModel model)
        {
            var result = await _sceneService.Share(User.Id(), sceneId, model.Keyword);
            return new ResponseDto<string>(result);
        }

        [AllowAnonymous]
        [HttpGet("share/{token}")]
        public async Task<ResponseDto<SharedResultItemModel>> GetSceneBySharedId(string token)
        {
            var result = await _searchService.GetSharedItem(token);
            return new ResponseDto<SharedResultItemModel>(result);
        }

        [HttpPost("report/{sceneId}")]
        public async Task<ResponseDto<Null>> ReportScene(Guid sceneId, [FromBody] ReportSceneModel model)
        {
            await _sceneService.Report(User.Id(), sceneId, model.Type);
            return new ResponseDto<Null>();
        }

        [HttpPost("bookmark/{sceneId}")]
        public async Task<ResponseDto<UserBookmarkDto>> BookmarkScene(Guid sceneId)
        {
            var result = await _sceneService.Bookmark(User.Id(), sceneId);
            return new ResponseDto<UserBookmarkDto>(_mapper.Map<UserBookmarkDto>(result));
        }

        [HttpDelete("bookmark/{sceneId}")]
        public async Task<ResponseDto<Null>> UnBookmarkScene(Guid sceneId)
        {
            await _sceneService.UnBookmark(User.Id(), sceneId);
            return new ResponseDto<Null>();
        }
    }
}

