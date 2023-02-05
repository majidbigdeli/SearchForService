using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SearchForApi.Models.Dtos;
using SearchForApi.Models.Exceptions;
using SearchForApi.Models.Inputs;
using SearchForApi.Services;
using SearchForApi.Utilities;

namespace SearchForApi.Controllers
{
    [ApiController]
    [Authorize(Roles = "Administrator")]
    [Route("admin")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class AdminController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILimitationService _limitationService;

        public AdminController(IMapper mapper, ILimitationService limitationService)
        {
            _mapper = mapper;
            _limitationService = limitationService;
        }

        [HttpGet("movie-assets")]
        public async Task<SinglePaginationResponseDto<MovieAssetDto>> GetUncheckedMovieAssets()
        {
            var (total, result) = await _limitationService.GetUncheckedMovieAsset();
            return new SinglePaginationResponseDto<MovieAssetDto>(total, result);
        }

        [HttpPut("movie-assets/check")]
        public async Task<ResponseDto<Null>> CheckedMovieAssets([FromBody] CheckMovieAssetsModel model)
        {
            if (!ModelState.IsValid)
                throw new ValidationException();

            await _limitationService.CheckdMovieAsset(User.Id(), model.MovieId, model.Status);
            return new ResponseDto<Null>();
        }

        [HttpPut("scene/{sceneId}/check")]
        public async Task<ResponseDto<Null>> CheckScene(Guid sceneId, [FromBody] ExcludeSceneModel model)
        {
            if (!ModelState.IsValid)
                throw new ValidationException();

            await _limitationService.Check(User.Id(), sceneId, model.Type, model.ResultType, model.ExcludeType);
            return new ResponseDto<Null>();
        }
    }
}