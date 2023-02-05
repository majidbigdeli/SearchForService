using System;
using System.Threading.Tasks;
using MethodTimer;
using Microsoft.AspNetCore.Identity;
using SearchForApi.Core;
using SearchForApi.Factories;
using SearchForApi.Integrations.Kavehnegar;
using SearchForApi.Integrations.SearchForModules;
using SearchForApi.Integrations.Sendinblue;
using SearchForApi.Models.DatabaseContext;
using SearchForApi.Models.Dtos;
using SearchForApi.Models.Entities;
using SearchForApi.Models.Exceptions;
using SearchForApi.Models.Inputs;
using SearchForApi.Repositories;
using SearchForApi.Utilities;
using SearchForApi.Utilities.LockManager;

namespace SearchForApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IAuthFactory _authFactory;
        private readonly ISendinblueIntegration _sendinblueIntegration;
        private readonly IUserService _userService;
        private readonly ILinkFactory _linkFactory;
        private readonly AssetRepository _assetRepository;
        private readonly IUserFactory _userFactory;
        private readonly IGoogleTokenValidator _googleTokenValidator;
        private readonly IKavehnegarIntegration _kavehnegarIntegration;

        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, IAuthFactory authFactory, ISendinblueIntegration sendinblueIntegration, IUserService userService, ILinkFactory linkFactory, AssetRepository assetRepository, IUserFactory userFactory, IGoogleTokenValidator googleTokenValidator, IKavehnegarIntegration kavehnegarIntegration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authFactory = authFactory;
            _sendinblueIntegration = sendinblueIntegration;
            _userService = userService;
            _linkFactory = linkFactory;
            _assetRepository = assetRepository;
            _userFactory = userFactory;
            _googleTokenValidator = googleTokenValidator;
            _kavehnegarIntegration = kavehnegarIntegration;
        }

        [Time("platform={platform}")]
        public Task<TokenDto> GoogleLogin(string IdToken, PlatformType platform, DeviceModel device)
        {
            throw new NotImplementedException();

            //var payload = await _googleTokenValidator.Validate(IdToken);
            //using (var lockManager = new RegisterLockManager())
            //{
            //    await lockManager.AcquireLock(payload.Email);

            //    var user = await ((ApiUserManager)_userManager).FindByNameWithDevicesAsync(payload.Email);
            //    if (user == null)
            //    {
            //        user = _authFactory.CreateNewGoogleUserInstance(payload.Email, payload.Name, platform, device);
            //        var result = await _userManager.CreateAsync(user);
            //        _authFactory.CheckIdentityResult(result);
            //    }
            //    else if (!user.EmailConfirmed)
            //    {
            //        user = _authFactory.ResetUnverifiedGoogleEmailUserInstance(user, platform, payload.Name, device);
            //        var result = await _userManager.UpdateAsync(user);
            //        _authFactory.CheckIdentityResult(result);
            //    }
            //    else if (device != null)
            //        await _userService.AddOrUpdateUserDevice(user, device);

            //    // fixme:
            //    //var (avatarStorageId, avatarContentType) = _userFactory.GenerateUserAvatarFileName(user);
            //    //await _assetRepository.UploadUrl(avatarStorageId, payload.Picture, avatarContentType, true);

            //    //_ = _userService.AddOrUpdateUserContact(user, new { Plan = (int)user.PlanId, Name = payload.Name });

            //    var authToken = await _authFactory.CreateNewTokenInstance(user);
            //    return authToken;
            //}
        }

        [Time("username={username}")]
        public async Task ChangePassword(string username, string newPassword)
        {
            var existUser = await _userManager.FindByNameAsync(username);
            if (existUser == null)
                throw new ValidationException();

            var resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(existUser);
            var result = await _userManager.ResetPasswordAsync(existUser, resetPasswordToken, newPassword);
            _authFactory.CheckIdentityResult(result);
        }

        [Time("email={email}")]
        public async Task ForgetPassword(string email)
        {
            var existUser = await _userManager.FindByNameAsync(email);
            if (existUser == null)
                return;

            var confirmationToken = await _userManager.GeneratePasswordResetTokenAsync(existUser);
            var resetPasswordUrl = _linkFactory.GetRestPasswordUrl(email, confirmationToken);
            _ = _sendinblueIntegration.SendEmail(email, SendinBlueEmailTemplates.ResetPassword, new { link = resetPasswordUrl });
        }

        [Time("email={email},token={token}")]
        public async Task ConfirmForgetPassword(string email, string token, string newPassword)
        {
            var existUser = await _userManager.FindByNameAsync(email);
            if (existUser == null)
                throw new ConfirmationTokenIsNotCorrectException();

            var result = await _userManager.ResetPasswordAsync(existUser, token, newPassword);
            if (!result.Succeeded)
                throw new ConfirmationTokenIsNotCorrectException();

            if (!existUser.EmailConfirmed)
            {
                var emailConfirmToken = await _userManager.GenerateEmailConfirmationTokenAsync(existUser);
                var confirmationResult = await _userManager.ConfirmEmailAsync(existUser, emailConfirmToken);
                _authFactory.CheckIdentityResult(confirmationResult);
            }
        }

        [Time("username={username}")]
        public async Task<TokenDto> Login(string username, string password, DeviceModel device)
        {
            var existUser = await ((ApiUserManager)_userManager).FindByNameWithDevicesAsync(username);
            if (existUser == null)
                throw new IncorrectAuthanticationException();

            var result = await _signInManager.PasswordSignInAsync(existUser, password, false, true);
            if (result.IsLockedOut)
                throw new AccountTemporarilyLockedException();
            if (result.IsNotAllowed)
                throw new EmailIsNotConfirmedException();
            if (!result.Succeeded)
                throw new IncorrectAuthanticationException();

            if (device != null)
                await _userService.AddOrUpdateUserDevice(existUser, device);

            var authToken = await _authFactory.CreateNewTokenInstance(existUser);
            return authToken;
        }

        [Time("email={email},platform={platform}")]
        public async Task Register(string email, string password, PlatformType platform, DeviceModel device)
        {
            using (var lockManager = new RegisterLockManager())
            {
                await lockManager.AcquireLock(email);

                var user = await ((ApiUserManager)_userManager).FindByNameWithDevicesAsync(email);
                if (user == null)
                {
                    user = _authFactory.CreateNewUserByEmailInstance(email, platform, device);
                    var result = await _userManager.CreateAsync(user, password);
                    _authFactory.CheckIdentityResult(result);
                }
                else if (!user.EmailConfirmed)
                {
                    user = _authFactory.ResetUnverifiedEmailUserInstance(user, password, platform, device);
                    var result = await _userManager.UpdateAsync(user);
                    _authFactory.CheckIdentityResult(result);
                }
                else
                    throw new DuplicateUserException();

                var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmEmailUrl = _linkFactory.GetConfirmEmailUrl(email, confirmationToken);

                _ = _sendinblueIntegration.SendEmail(email, SendinBlueEmailTemplates.EmailVerification, new { link = confirmEmailUrl });
            }
        }

        [Time("phoneNumber={phoneNumber},platform={platform}")]
        public async Task RegisterWithPhoneNumber(string phoneNumber, PlatformType platform, DeviceModel device)
        {
            phoneNumber = phoneNumber.NormalizePhoneNumber();

            using (var lockManager = new RegisterLockManager())
            {
                await lockManager.AcquireLock(phoneNumber);

                var user = await ((ApiUserManager)_userManager).FindByNameWithDevicesAsync(phoneNumber);
                if (user == null)
                {
                    user = _authFactory.CreateNewUserByPhoneNumberInstance(phoneNumber, platform, device);
                    var result = await _userManager.CreateAsync(user);
                    _authFactory.CheckIdentityResult(result);
                }
                else if (!user.PhoneNumberConfirmed)
                {
                    user = _authFactory.ResetUnverifiedPhoneNumberUserInstance(user, platform, device);
                    var result = await _userManager.UpdateAsync(user);
                    _authFactory.CheckIdentityResult(result);
                }

                var confirmationToken = await _userManager.GenerateChangePhoneNumberTokenAsync(user, phoneNumber);

                _ = _kavehnegarIntegration.SendVerificationToken(phoneNumber, confirmationToken);
            }
        }

        [Time("username={username},token={token}")]
        public async Task<string> ConfirmEmail(string username, string token)
        {
            var existUser = await _userManager.FindByNameAsync(username);
            if (existUser == null)
                throw new ConfirmationTokenIsNotCorrectException();

            var result = await _userManager.ConfirmEmailAsync(existUser, token);
            if (!result.Succeeded)
                throw new ConfirmationTokenIsNotCorrectException();

            //_ = _userService.AddOrUpdateUserContact(existUser, new { Plan = (int)existUser.PlanId });

            return Cfg.EmailVerificationResultUrl;
        }

        [Time("username={username},token={token}")]
        public async Task<TokenDto> ConfirmPhoneNumber(string username, string token, DeviceModel device)
        {
            username = username.NormalizePhoneNumber();

            var existUser = await ((ApiUserManager)_userManager).FindByNameWithDevicesAsync(username);
            if (existUser == null)
                throw new ConfirmationTokenIsNotCorrectException();

            var result = await _userManager.ChangePhoneNumberAsync(existUser, username, token);
            if (!result.Succeeded)
                throw new ConfirmationTokenIsNotCorrectException();

            if (device != null)
                await _userService.AddOrUpdateUserDevice(existUser, device);

            //_ = _userService.AddOrUpdateUserContact(existUser, new { Plan = (int)existUser.PlanId, SMS = existUser.PhoneNumber.NationalizePhoneNumber() });

            var authToken = await _authFactory.CreateNewTokenInstance(existUser);
            return authToken;
        }

        [Time("email={email}")]
        public async Task ResendEmailConfirmationCode(string email)
        {
            var existUser = await _userManager.FindByNameAsync(email);
            if (existUser == null)
                return;

            var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(existUser);
            var confirmEmailUrl = _linkFactory.GetConfirmEmailUrl(email, confirmationToken);

            _ = _sendinblueIntegration.SendEmail(email, SendinBlueEmailTemplates.EmailVerification, new { link = confirmEmailUrl });
        }

        [Time("phoneNumber={phoneNumber}")]
        public async Task ResendPhoneNumberConfirmationCode(string phoneNumber)
        {
            phoneNumber = phoneNumber.NormalizePhoneNumber();

            var existUser = await _userManager.FindByNameAsync(phoneNumber);
            if (existUser == null)
                return;

            var confirmationToken = await _userManager.GenerateChangePhoneNumberTokenAsync(existUser, phoneNumber);

            _ = _kavehnegarIntegration.SendVerificationToken(phoneNumber, confirmationToken);
        }

        public async Task LogOut(Guid userId, string deviceId)
        {
            await _userService.DeactiveUserDevice(userId, deviceId);
        }
    }

    public class RegisterLockManager : LockManager<string>
    {
        public RegisterLockManager() : base("Register") { }
    }
}

