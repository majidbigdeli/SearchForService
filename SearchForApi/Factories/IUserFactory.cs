using System;
using System.Threading.Tasks;
using SearchForApi.Models;
using SearchForApi.Models.Entities;
using SearchForApi.Models.Inputs;

namespace SearchForApi.Factories
{
    public interface IUserFactory
    {
        UserDevice CreateNewUserDeviceInstance(DeviceModel device, bool isActive);
        void ChangeUserDeviceActiveStatus(UserDevice device, bool isActive);
        void SetUserPlan(User user, PlanType type, bool is3Months, DateTime? oldExpireDate = null);
        void SetUserPlan(User user, PlanType type, DateTime planChangedOn, DateTime? expireDate = null);
        Task<UserPlanStatusModel> CreateNewUserPlanStatusInstance(User user, int bookmarkCount, int shareCount);
        int? GetUserExpireDays(User user);
        (string filename, string contentType) GenerateUserAvatarFileName(User user);
        UserDevice UpdateUserDeviceInstance(UserDevice existDevice, DeviceModel device);
    }
}