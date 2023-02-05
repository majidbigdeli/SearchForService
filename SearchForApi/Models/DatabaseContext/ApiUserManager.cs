using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SearchForApi.Models.Entities;

namespace SearchForApi.Models.DatabaseContext
{
    public class ApiUserManager : UserManager<User>
    {
        public ApiUserManager(IUserStore<User> userStore, IOptions<IdentityOptions> optionsAccessor,
         IPasswordHasher<User> passwordHasher,
         IEnumerable<IUserValidator<User>> userValidators,
         IEnumerable<IPasswordValidator<User>> passwordValidators, ILookupNormalizer keyNormalizer,
         IdentityErrorDescriber errors, IServiceProvider services, ILogger<ApiUserManager> logger) :
         base(userStore, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors,
             services, logger)
        {
        }

        public Task<User> FindByNameWithDevicesAsync(string userName, CancellationToken cancellationToken = default)
        {
            return ((ApiUserStore)Store).FindByNameWithDevicesAsync(NormalizeName(userName), cancellationToken);
        }

        public Task<User> FindByIdWithDevicesAsync(string userId, CancellationToken cancellationToken = default)
        {
            return ((ApiUserStore)Store).FindByIdWithDevicesAsync(userId, cancellationToken);
        }

        public Task<User> FindByIdWithPlanAsync(string userId, CancellationToken cancellationToken = default)
        {
            return ((ApiUserStore)Store).FindByIdWithPlanAsync(userId, cancellationToken);
        }
    }
}