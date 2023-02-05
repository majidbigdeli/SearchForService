using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MethodTimer;
using Microsoft.AspNetCore.Identity;
using SearchForApi.Core;
using SearchForApi.Factories;
using SearchForApi.Integrations.Sendinblue;
using SearchForApi.Models;
using SearchForApi.Models.DatabaseContext;
using SearchForApi.Models.Entities;
using SearchForApi.Models.Exceptions;
using SearchForApi.Models.Inputs;
using SearchForApi.Repositories;
using SearchForApi.Utilities;
using SearchForApi.Utilities.LockManager;
using sibm = sib_api_v3_sdk.Model;

namespace SearchForApi.Services
{
    public class UserService : IUserService
    {
        private readonly ISendinblueIntegration _sendinblueIntegration;
        private readonly UserManager<User> _userManager;
        private readonly IDateTimeFactory _dateTimeFactory;
        private readonly IUserFactory _userFactory;
        private readonly ICacheService _cacheService;
        private readonly BookmarkRepository _bookmarkRepository;
        private readonly HistoryRepository _historyRepository;
        private readonly ShareRepository _shareRepository;
        private readonly UserDeviceRepository _userDeviceRepository;

        private static string GetUserPlanStatusCacheItemKey(Guid userId) => $"USER_PLAN_STATUS_{userId}";

        public UserService(ISendinblueIntegration sendinblueIntegration, UserManager<User> userManager, IDateTimeFactory dateTimeFactory, IUserFactory userFactory, ICacheService cacheService, BookmarkRepository bookmarkRepository, HistoryRepository historyRepository, ShareRepository shareRepository, UserDeviceRepository userDeviceRepository)
        {
            _sendinblueIntegration = sendinblueIntegration;
            _userManager = userManager;
            _dateTimeFactory = dateTimeFactory;
            _userFactory = userFactory;
            _cacheService = cacheService;
            _bookmarkRepository = bookmarkRepository;
            _historyRepository = historyRepository;
            _shareRepository = shareRepository;
            _userDeviceRepository = userDeviceRepository;
        }

        [Time("userId={userId}")]
        public async Task<UserPlanStatusModel> GetUserCurrentPlanStatus(Guid userId)
        {
            var cacheKey = GetUserPlanStatusCacheItemKey(userId);
            using (var lockManager = new UserPlanLockManager())
            {
                await lockManager.AcquireLock(cacheKey);

                var cachedItem = await _cacheService.GetItem<UserPlanStatusModel>(cacheKey);
                if (cachedItem == null)
                {
                    var existUser = await _userManager.FindByIdAsync(userId.ToString());
                    var bookmarkCount = await _bookmarkRepository.GetCountByUserId(userId);
                    var shareCount = await _shareRepository.GetShareCountByUserId(userId);

                    cachedItem = await _userFactory.CreateNewUserPlanStatusInstance(existUser, bookmarkCount, shareCount);
                    await _cacheService.SetItem(cacheKey, cachedItem, cachedItem.CacheLifeTime);
                }

                return cachedItem;
            }
        }

        [Time("userId={userId}")]
        public async Task SetUserCurrentPlanStatus(Guid userId, UserPlanStatusModel newStatus)
        {
            var cacheKey = GetUserPlanStatusCacheItemKey(userId);
            await _cacheService.SetItem(cacheKey, newStatus, newStatus.CacheLifeTime);
        }

        [Time("userId={userId}")]
        public async Task RemoveUserCurrentPlanStatus(Guid userId)
        {
            var cacheKey = GetUserPlanStatusCacheItemKey(userId);
            await _cacheService.RemoveItem(cacheKey);
        }

        [Time]
        public async Task AddOrUpdateUserDevice(User existUser, DeviceModel device)
        {
            var existDevice = existUser.Devices.SingleOrDefault(p => p.DeviceId == device.Id);
            if (existDevice != null)
                _userFactory.ChangeUserDeviceActiveStatus(existDevice, true);
            else
                existUser.Devices.Add(_userFactory.CreateNewUserDeviceInstance(device, true));

            var activeDeviceCount = existUser.Devices.Count(p => p.IsActive);
            if (activeDeviceCount > Cfg.MaxActiveDevicesPerUser)
            {
                // disbale old devices
                var oldDevices = existUser.Devices.Where(p => p.DeviceId != device.Id);
                foreach (var oldDevice in oldDevices)
                    _userFactory.ChangeUserDeviceActiveStatus(oldDevice, false);
            }

            var result = await _userManager.UpdateAsync(existUser);
            if (!result.Succeeded)
                throw new Exception(result.Errors.ToLogString());
        }

        [Time("userId={userId}")]
        public async Task UpdateUserDevice(Guid userId, DeviceModel device)
        {
            var existDevice = await _userDeviceRepository.GetUserDeviceByAndroidId(userId, device.Id);
            if (existDevice == null)
                throw new ValidationException();

            _userFactory.UpdateUserDeviceInstance(existDevice, device);

            await _userDeviceRepository.SaveChanges();
        }

        [Time("userId={userId},deviceId={deviceId}")]
        public async Task DeactiveUserDevice(Guid userId, string deviceId)
        {
            var existUser = await ((ApiUserManager)_userManager).FindByIdWithDevicesAsync(userId.ToString());

            var existDevice = existUser.Devices.SingleOrDefault(p => p.DeviceId == deviceId);
            if (existDevice == null)
                throw new ValidationException();

            _userFactory.ChangeUserDeviceActiveStatus(existDevice, false);

            var result = await _userManager.UpdateAsync(existUser);
            if (!result.Succeeded)
                throw new Exception(result.Errors.ToLogString());
        }

        [Time("userId={userId}")]
        public async Task<sibm.CreateUpdateContactModel> AddOrUpdateUserContact(Guid userId, object attributes = null)
        {
            var existUser = await _userManager.FindByIdAsync(userId.ToString());
            if (!existUser.EmailConfirmed && !existUser.PhoneNumberConfirmed)
                throw new EmailIsNotConfirmedException();

            var result = await _sendinblueIntegration.CreateOrUpdateContact(existUser.Email, attributes, new List<SendinBlueContactLists?>() { SendinBlueContactLists.Users });
            return result;
        }

        [Time("user={user}")]
        public async Task<sibm.CreateUpdateContactModel> AddOrUpdateUserContact(User user, object attributes = null)
        {
            if (!user.EmailConfirmed && !user.PhoneNumberConfirmed)
                throw new EmailIsNotConfirmedException();

            var result = await _sendinblueIntegration.CreateOrUpdateContact(user.Email, attributes, new List<SendinBlueContactLists?>() { SendinBlueContactLists.Users });
            return result;
        }

        [Time("userId={userId}")]
        public async Task<User> GetUserById(Guid userId)
        {
            return await _userManager.FindByIdAsync(userId.ToString());
        }

        [Time("userId={userId}")]
        public async Task<User> GetUserProfile(Guid userId)
        {
            var existUser = await _userManager.FindByIdAsync(userId.ToString());
            return existUser;
        }

        [Time("userId={userId},deviceId={deviceId}")]
        public async Task<UserDeviceStatus> GetDeviceStatus(Guid userId, string deviceId)
        {
            var userDevice = await _userDeviceRepository.GetUserDeviceByAndroidId(userId, deviceId);
            if (userDevice == null || !userDevice.IsActive)
                return UserDeviceStatus.Deactive;

            return UserDeviceStatus.Active;
        }

        [Time("userId={userId},planType={planType},is3Months={is3Months}")]
        public async Task<(User user, PlanType oldPlanType, DateTime oldPlanChangedOn, DateTime? oldPlanExpireDate)> SetUserCurrentPlan(Guid userId, PlanType planType, bool is3Months)
        {
            var existUser = await _userManager.FindByIdAsync(userId.ToString());
            var oldPlanType = existUser.PlanId;
            var oldPlanExpireDate = existUser.PlanExpireDate;
            var oldPlanChangedOn = existUser.PlanChangedOn;

            _userFactory.SetUserPlan(existUser, planType, is3Months, existUser.PlanExpireDate);

            var result = await _userManager.UpdateAsync(existUser);
            if (!result.Succeeded)
                throw new Exception(result.Errors.ToLogString());

            return (existUser, oldPlanType, oldPlanChangedOn, oldPlanExpireDate);
        }

        [Time("planType={planType},expireDate={expireDate}")]
        public async Task SetUserCurrentPlan(User user, PlanType planType, DateTime planChangedOn, DateTime? expireDate = null)
        {
            _userFactory.SetUserPlan(user, planType, planChangedOn, expireDate);

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new Exception(result.Errors.ToLogString());
        }
    }

    public class UserPlanLockManager : LockManager<string>
    {
        public UserPlanLockManager() : base("UserPlan") { }
    }
}