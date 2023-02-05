using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SearchForApi.Factories;
using SearchForApi.Models.Dtos;
using SearchForApi.Models.Entities;
using SearchForApi.Services;
using SearchForApi.Utilities;

namespace SearchForApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("user")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IUserFactory _userFactory;
        private readonly ILinkFactory _linkFactory;

        public UserController(IUserService userService, IMapper mapper, IUserFactory userFactory, ILinkFactory linkFactory)
        {
            _userService = userService;
            _mapper = mapper;
            _userFactory = userFactory;
            _linkFactory = linkFactory;
        }

        [MapToApiVersion("1.0")]
        [HttpGet]
        public async Task<ResponseDto<UserProfileDto>> Get()
        {
            var user = await _userService.GetUserProfile(User.Id());
            var userPlanStatus = await _userService.GetUserCurrentPlanStatus(User.Id());

            var normalizedResult = _mapper.Map<User, UserProfileDto>(user, opt =>
                 opt.AfterMap((src, dest) =>
                 {
                     dest.PlanType = user.ActualPlanId;
                     dest.ExpireDays = _userFactory.GetUserExpireDays(src);
                     dest.PhoneNumber = src.PhoneNumber?.MaskPhoneNumber();
                     dest.AvatarImageUrl = _linkFactory.GetUserAvatarImageUrl(src.StorageId);
                     dest.TrialDaysRemain = userPlanStatus.TrialDaysRemain;
                 })
             );

            return new ResponseDto<UserProfileDto>(normalizedResult);
        }

        [MapToApiVersion("2.0")]
        [HttpGet]
        public async Task<ResponseDto<UserProfileDto>> Get_v2()
        {
            var user = await _userService.GetUserProfile(User.Id());
            var userPlanStatus = await _userService.GetUserCurrentPlanStatus(User.Id());

            var normalizedResult = _mapper.Map<User, UserProfileDto>(user, opt =>
                 opt.AfterMap((src, dest) =>
                 {
                     dest.PlanType = userPlanStatus.Type;
                     dest.ExpireDays = _userFactory.GetUserExpireDays(src);
                     dest.PhoneNumber = src.PhoneNumber?.MaskPhoneNumber();
                     dest.AvatarImageUrl = _linkFactory.GetUserAvatarImageUrl(src.StorageId);
                     dest.TrialDaysRemain = userPlanStatus.TrialDaysRemain;
                 })
             );

            return new ResponseDto<UserProfileDto>(normalizedResult);
        }

        [HttpGet("device/status/{deviceId}")]
        public async Task<ResponseDto<UserDeviceStatus>> GetDeviceStatus(string deviceId)
        {
            var result = await _userService.GetDeviceStatus(User.Id(), deviceId);
            return new ResponseDto<UserDeviceStatus>(result);
        }
    }
}