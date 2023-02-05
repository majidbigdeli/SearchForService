using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using MethodTimer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using SearchForApi.Core;
using SearchForApi.Models.Dtos;
using SearchForApi.Models.Entities;
using SearchForApi.Models.Inputs;
using SearchForApi.Utilities;

namespace SearchForApi.Factories
{
    public class AuthFactory : IAuthFactory
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserFactory _userFactory;
        private readonly IDateTimeFactory _dateTimeFactory;

        public AuthFactory(UserManager<User> userManager, IUserFactory userFactory, IDateTimeFactory dateTimeFactory)
        {
            _userManager = userManager;
            _userFactory = userFactory;
            _dateTimeFactory = dateTimeFactory;
        }

        [Time]
        public async Task<TokenDto> CreateNewTokenInstance(User user)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var userRole in userRoles)
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));

            var token = new JwtSecurityToken(
                issuer: Cfg.JWTValidIssuer,
                audience: Cfg.JWTValidAudience,
                expires: DateTime.UtcNow.AddYears(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(Cfg.JWTSecretKey, SecurityAlgorithms.HmacSha256)
            );

            return new TokenDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpireDate = token.ValidTo
            };
        }

        public User CreateNewUserByEmailInstance(string email, PlatformType platform, DeviceModel device)
        {
            var newUser = new User
            {
                Id = Guid.NewGuid(),
                StorageId = ObjectId.GenerateNewId().ToString(),
                UserName = email,
                Email = email,
                Name = email.Split('@')[0],
                SecurityStamp = Guid.NewGuid().ToString(),
                CreatedOn = _dateTimeFactory.UtcNow,
                PlanId = PlanType.Basic,
                PlanChangedOn = _dateTimeFactory.UtcNow,
                RegisterMethod = RegisterMethodType.Email,
                RegisterPlatform = platform,
                Devices = new List<UserDevice>()
            };

            if (device != null)
                newUser.Devices.Add(_userFactory.CreateNewUserDeviceInstance(device, false));

            return newUser;
        }

        public User CreateNewUserByPhoneNumberInstance(string phoneNumber, PlatformType platform, DeviceModel device)
        {
            var newUser = new User
            {
                Id = Guid.NewGuid(),
                StorageId = ObjectId.GenerateNewId().ToString(),
                UserName = phoneNumber,
                PhoneNumber = phoneNumber,
                SecurityStamp = Guid.NewGuid().ToString(),
                CreatedOn = _dateTimeFactory.UtcNow,
                PlanId = PlanType.Basic,
                PlanChangedOn = _dateTimeFactory.UtcNow,
                RegisterMethod = RegisterMethodType.PhoneNumber,
                RegisterPlatform = platform,
                Devices = new List<UserDevice>()
            };

            if (device != null)
                newUser.Devices.Add(_userFactory.CreateNewUserDeviceInstance(device, false));

            return newUser;
        }

        public User ResetUnverifiedEmailUserInstance(User user, string password, PlatformType platform, DeviceModel device)
        {
            user.RegisterPlatform = platform;

            var passwordHash = _userManager.PasswordHasher.HashPassword(user, password);
            user.PasswordHash = passwordHash;

            user.Devices.Clear();
            if (device != null)
                user.Devices.Add(_userFactory.CreateNewUserDeviceInstance(device, false));

            return user;
        }

        public User ResetUnverifiedGoogleEmailUserInstance(User user, PlatformType platform, string name, DeviceModel device)
        {
            user.Name = name;
            user.RegisterPlatform = platform;
            user.PasswordHash = null;
            user.EmailConfirmed = true;

            user.Devices.Clear();
            if (device != null)
                user.Devices.Add(_userFactory.CreateNewUserDeviceInstance(device, true));

            return user;
        }

        public User ResetUnverifiedPhoneNumberUserInstance(User user, PlatformType platform, DeviceModel device)
        {
            user.RegisterPlatform = platform;

            user.Devices.Clear();
            if (device != null)
                user.Devices.Add(_userFactory.CreateNewUserDeviceInstance(device, false));

            return user;
        }

        public User CreateNewGoogleUserInstance(string email, string name, PlatformType platform, DeviceModel device)
        {
            var newUser = new User
            {
                Id = Guid.NewGuid(),
                StorageId = ObjectId.GenerateNewId().ToString(),
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                Name = name,
                SecurityStamp = Guid.NewGuid().ToString(),
                CreatedOn = _dateTimeFactory.UtcNow,
                PlanId = PlanType.Basic,
                PlanChangedOn = _dateTimeFactory.UtcNow,
                RegisterMethod = RegisterMethodType.Google,
                RegisterPlatform = platform,
                Devices = new List<UserDevice>()
            };

            if (device != null)
                newUser.Devices.Add(_userFactory.CreateNewUserDeviceInstance(device, true));

            return newUser;
        }

        public void CheckIdentityResult(IdentityResult result)
        {
            if (!result.Succeeded)
                throw new Exception(result.Errors.ToLogString());
        }
    }
}

