using System;
using System.Threading.Tasks;
using SearchForApi.Models.Dtos;
using SearchForApi.Models.Entities;
using SearchForApi.Models.Inputs;

namespace SearchForApi.Services
{
    public interface IAuthService
    {
        Task Register(string email, string password, PlatformType platform, DeviceModel device);
        Task<TokenDto> Login(string username, string password, DeviceModel device);
        Task<string> ConfirmEmail(string username, string token);
        Task ChangePassword(string username, string newPassword);
        Task ForgetPassword(string email);
        Task ConfirmForgetPassword(string email, string token, string newPassword);
        Task ResendEmailConfirmationCode(string email);
        Task LogOut(Guid userId, string deviceId);
        Task<TokenDto> GoogleLogin(string IdToken, PlatformType platform, DeviceModel device);
        Task RegisterWithPhoneNumber(string phoneNumber, PlatformType platform, DeviceModel device);
        Task<TokenDto> ConfirmPhoneNumber(string username, string token, DeviceModel device);
        Task ResendPhoneNumberConfirmationCode(string phoneNumber);
    }
}

