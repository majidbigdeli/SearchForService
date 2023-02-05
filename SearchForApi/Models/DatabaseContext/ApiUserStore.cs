using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SearchForApi.Models.Entities;

namespace SearchForApi.Models.DatabaseContext
{
    public class ApiUserStore : UserStore<User, IdentityRole<Guid>, ApiContext, Guid>
    {
        public ApiUserStore(ApiContext dbContext, IdentityErrorDescriber identityErrorDescriber)
            : base(dbContext, identityErrorDescriber)
        { }

        public Task<User> FindByNameWithDevicesAsync(string normalizedUserName, CancellationToken cancellationToken = default)
        {
            return Context.Users
                .Include(p => p.Devices)
                .FirstOrDefaultAsync(q => q.NormalizedUserName == normalizedUserName, cancellationToken);
        }

        public Task<User> FindByIdWithDevicesAsync(string userId, CancellationToken cancellationToken = default)
        {
            return Context.Users
                .Include(p => p.Devices)
                .FirstOrDefaultAsync(q => q.Id == Guid.Parse(userId), cancellationToken);
        }

        public Task<User> FindByIdWithPlanAsync(string userId, CancellationToken cancellationToken = default)
        {
            return Context.Users
                .Include(p => p.Plan)
                .FirstOrDefaultAsync(q => q.Id == Guid.Parse(userId), cancellationToken);
        }
    }
}