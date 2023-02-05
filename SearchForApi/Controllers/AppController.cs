using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SearchForApi.Factories;
using SearchForApi.Models.Dtos;
using SearchForApi.Models.Entities;
using SearchForApi.Models.Exceptions;
using SearchForApi.Models.Inputs;
using SearchForApi.Repositories;
using SearchForApi.Services;
using SearchForApi.Utilities;

namespace SearchForApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("app")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class AppController : ControllerBase
    {
        private readonly AppReleaseRepository _appReleaseRepository;
        private readonly IMapper _mapper;
        private readonly ILinkFactory _linkFactory;
        private readonly IUserService _userService;
        private readonly IAppService _appService;

        public AppController(AppReleaseRepository appReleaseRepository, IMapper mapper, ILinkFactory linkFactory, IUserService userService, IAppService appService)
        {
            _appReleaseRepository = appReleaseRepository;
            _mapper = mapper;
            _linkFactory = linkFactory;
            _userService = userService;
            _appService = appService;
        }

        [HttpGet("release/new")]
        public async Task<ResponseDto<AppReleaseDto>> CheckForNewRelease()
        {
            var (result, forceUpdate) = await _appService.CheckForNewRelease(Request.AppVersion(), Request.Platform());

            return new ResponseDto<AppReleaseDto>(
                _mapper.Map<AppRelease, AppReleaseDto>(result, opt =>
                 opt.AfterMap((src, dest) =>
                 {
                     if (src != null)
                     {
                         dest.DownloadUrl = _linkFactory.GetAppReleaseDownload(src.Id);
                         dest.ForceUpdate = forceUpdate;
                     }
                 }))
            );
        }

        [AllowAnonymous]
        [HttpGet("release/latest")]
        public async Task<ResponseDto<AppReleaseDto>> GetLatest()
        {
            var result = await _appReleaseRepository.GetLatestRelease(PlatformType.Android);

            return new ResponseDto<AppReleaseDto>(
                _mapper.Map<AppRelease, AppReleaseDto>(result, opt =>
                 opt.AfterMap((src, dest) =>
                 {
                     if (src != null)
                         dest.DownloadUrl = _linkFactory.GetAppReleaseDownload(src.Id);
                 }))
            );
        }

        [MapToApiVersion("2.0")]
        [HttpPut("device")]
        public async Task<ResponseDto<Null>> UpdateDevice([FromBody] DeviceModel model)
        {
            if (!ModelState.IsValid)
                throw new ValidationException();

            await _userService.UpdateUserDevice(User.Id(), model);

            return new ResponseDto<Null>();
        }
    }
}
