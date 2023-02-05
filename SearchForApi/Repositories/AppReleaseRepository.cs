using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MethodTimer;
using Microsoft.EntityFrameworkCore;
using SearchForApi.Models.DatabaseContext;
using SearchForApi.Models.Entities;

namespace SearchForApi.Repositories
{
    public class AppReleaseRepository : BaseRepository<AppRelease, string>
    {
        public AppReleaseRepository(ApiContext context) : base(context)
        {
        }

        [Time("id={id}")]
        public override async Task<AppRelease> Get(string id)
        {
            return await _entities
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        [Time("currentVersion={currentVersion},platform={platform}")]
        public async Task<List<AppRelease>> GetNewerReleases(Version currentVersion, PlatformType platform)
        {
            return (await _entities
                .Where(p =>
                    p.Platform == platform &&
                    p.IsEnable &&
                    p.ReleasedOn != null)
                .ToListAsync())
                .Where(p => p.Version > currentVersion)
                .OrderBy(p => p.Version)
                .ToList();
        }

        [Time("currentVersion={currentVersion},platform={platform}")]
        public async Task<AppRelease> GetNewRelease(Version currentVersion, PlatformType platform)
        {
            return (await _entities
                .Where(p =>
                    p.Platform == platform &&
                    p.IsEnable &&
                    p.ReleasedOn != null)
                .ToListAsync())
                .OrderByDescending(p => p.Version)
                .FirstOrDefault(p => p.Version > currentVersion);
        }

        [Time("platform={platform}")]
        public async Task<AppRelease> GetLatestRelease(PlatformType platform)
        {
            return (await _entities
                .Where(p =>
                    p.Platform == platform &&
                    p.IsEnable &&
                    p.ReleasedOn != null)
                .ToListAsync())
                .OrderByDescending(p => p.Version)
                .FirstOrDefault();
        }
    }
}
