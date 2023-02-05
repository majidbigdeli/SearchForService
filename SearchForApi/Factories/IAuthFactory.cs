using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SearchForApi.Models.Dtos;
using SearchForApi.Models.Entities;
using SearchForApi.Models.Inputs;

namespace SearchForApi.Factories
{
    public interface IAuthFactory
    {
        Task<TokenDto> CreateNewTokenInstance(User user);
        User CreateNewUserByEmailInstance(string email, PlatformType platform, DeviceModel device);
        User CreateNewGoogleUserInstance(string email, string name, PlatformType platform, DeviceModel device);
        User ResetUnverifiedEmailUserInstance(User user, string password, PlatformType platform, DeviceModel device);
        User ResetUnverifiedGoogleEmailUserInstance(User user, PlatformType platform, string name, DeviceModel device);
        User CreateNewUserByPhoneNumberInstance(string phoneNumber, PlatformType platform, DeviceModel device);
        User ResetUnverifiedPhoneNumberUserInstance(User user, PlatformType platform, DeviceModel device);
        void CheckIdentityResult(IdentityResult result);
    }
}

