using System;
using System.Linq;
using System.Threading.Tasks;
using SearchForApi.Core;
using SearchForApi.Models;
using SearchForApi.Models.Entities;
using SearchForApi.Models.Inputs;
using SearchForApi.Repositories;

namespace SearchForApi.Factories
{
    public class UserFactory : IUserFactory
    {
        private readonly IDateTimeFactory _dateTimeFactory;
        private readonly PlanRepository _planRepository;

        public UserFactory(IDateTimeFactory dateTimeFactory, PlanRepository planRepository)
        {
            _dateTimeFactory = dateTimeFactory;
            _planRepository = planRepository;
        }

        public UserDevice CreateNewUserDeviceInstance(DeviceModel device, bool isActive = false)
        {
            var newDevice = new UserDevice
            {
                Id = Guid.NewGuid(),
                CreatedOn = _dateTimeFactory.UtcNow,
                ActiveStatusChangedOn = _dateTimeFactory.UtcNow,
                IsActive = isActive,

                AppVersion = device.AppVersion,
                DeviceId = device.Id,
                Model = device.Model,
                Os = device.Os,
                OsVersion = device.OsVersion,
                Platform = device.Platform,
                DataChangedOn = _dateTimeFactory.UtcNow,
            };

            return newDevice;
        }

        public UserDevice UpdateUserDeviceInstance(UserDevice existDevice, DeviceModel device)
        {
            existDevice.AppVersion = device.AppVersion;
            existDevice.Model = device.Model;
            existDevice.Os = device.Os;
            existDevice.OsVersion = device.OsVersion;
            existDevice.Platform = device.Platform;
            existDevice.NotificationToken = device.NotificationToken;
            existDevice.DataChangedOn = _dateTimeFactory.UtcNow;

            return existDevice;
        }

        public void ChangeUserDeviceActiveStatus(UserDevice device, bool isActive)
        {
            device.IsActive = isActive;
            device.ActiveStatusChangedOn = _dateTimeFactory.UtcNow;
        }

        public void SetUserPlan(User user, PlanType type, bool is3Months, DateTime? oldExpireDate = null)
        {
            var days = (is3Months ? 90 : 30) + 1;
            var currentExpireDate = new[] { _dateTimeFactory.UtcNow, oldExpireDate ?? _dateTimeFactory.UtcNow }.Max();
            var newExpireDate = currentExpireDate.AddDays(days);

            user.PlanId = type;
            user.PlanChangedOn = _dateTimeFactory.UtcNow;
            user.PlanExpireDate = newExpireDate;
        }

        public void SetUserPlan(User user, PlanType type, DateTime planChangedOn, DateTime? expireDate = null)
        {
            user.PlanId = type;
            user.PlanChangedOn = planChangedOn;
            user.PlanExpireDate = expireDate;
        }

        public async Task<UserPlanStatusModel> CreateNewUserPlanStatusInstance(User user, int bookmarkCount, int shareCount)
        {
            var plan = await _planRepository.Get(user.ActualPlanId);
            var trialDaysRemain = (int)Math.Max(0, (user.CreatedOn.AddDays(Cfg.TrialDays) - _dateTimeFactory.UtcNow).TotalDays);

            return new UserPlanStatusModel()
            {
                Type = plan.Id,
                SearchPerDayRemain = (plan.SearchPerDay ?? Cfg.MaxSearchPerDay),
                QuoteViewPerSearch = plan.QuoteViewPerSearch ?? Cfg.MaxQuoteViewPerSearch,
                ShareQuoteRemain = (plan.ShareQuote ?? Cfg.MaxShareQuote) - shareCount,
                BookmarkedRemain = (plan.BookmarkedCount ?? Cfg.MaxBookmarkedCount) - bookmarkCount,
                LastHistoriesDays = plan.LastHistoriesDays ?? Cfg.MaxLastHistoriesDays,
                KidsMode = plan.KidsMode,
                CacheLifeTime = _dateTimeFactory.UtcNow.AddDays(1),
                TrialDaysRemain = trialDaysRemain
            };
        }

        public int? GetUserExpireDays(User user)
        {
            int? expireDays = user.PlanExpireDate != null ?
                     (int)Math.Max(0, (user.PlanExpireDate - _dateTimeFactory.UtcNow).Value.TotalDays) :
                     null;

            return expireDays;
        }

        public (string filename, string contentType) GenerateUserAvatarFileName(User user)
        {
            var result = $"user_avatar_{user.StorageId}";

            // var fileExtension = Path.GetExtension(avatarFilename);
            // string contentType = MimeTypeMap.GetMimeType(fileExtension);

            return (result, "image/jpeg");
        }
    }
}