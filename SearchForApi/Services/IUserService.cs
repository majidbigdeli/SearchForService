using System;
using System.Threading.Tasks;
using SearchForApi.Models;
using SearchForApi.Models.Entities;
using SearchForApi.Models.Inputs;
using sibm = sib_api_v3_sdk.Model;

namespace SearchForApi.Services
{
    public interface IUserService
    {
        Task<User> GetUserById(Guid userId);
        Task<(User user, PlanType oldPlanType, DateTime oldPlanChangedOn, DateTime? oldPlanExpireDate)> SetUserCurrentPlan(Guid userId, PlanType planType, bool is3Months);
        Task SetUserCurrentPlan(User user, PlanType planType, DateTime planChangedOn, DateTime? expireDate = null);
        Task<sibm.CreateUpdateContactModel> AddOrUpdateUserContact(User user, object attributes = null);
        Task<sibm.CreateUpdateContactModel> AddOrUpdateUserContact(Guid userId, object attributes = null);
        Task AddOrUpdateUserDevice(User existUser, DeviceModel device);
        Task DeactiveUserDevice(Guid userId, string deviceId);
        Task<User> GetUserProfile(Guid userId);
        Task<UserDeviceStatus> GetDeviceStatus(Guid userId, string deviceId);
        Task<UserPlanStatusModel> GetUserCurrentPlanStatus(Guid userId);
        Task RemoveUserCurrentPlanStatus(Guid userId);
        Task SetUserCurrentPlanStatus(Guid userId, UserPlanStatusModel newStatus);
        Task UpdateUserDevice(Guid userId, DeviceModel device);
    }
}