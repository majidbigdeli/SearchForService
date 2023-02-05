using System;
using System.Linq;
using System.Threading.Tasks;
using SearchForApi.Models.Entities;
using SearchForApi.Repositories;

namespace SearchForApi.Services
{
    public class AppService : IAppService
    {
        private readonly AppReleaseRepository _appReleaseRepository;

        public AppService(AppReleaseRepository appReleaseRepository)
        {
            _appReleaseRepository = appReleaseRepository;
        }

        public async Task<(AppRelease newRelease, bool forceUpdate)> CheckForNewRelease(Version version, PlatformType platformType)
        {
            var newerReleases = await _appReleaseRepository.GetNewerReleases(version, platformType);
            if (newerReleases.Count == 0) return (null, false);

            var newestRelease = newerReleases.LastOrDefault();
            var forceUpdate = newerReleases.Any(p => p.ForceUpdate);

            return (newestRelease, forceUpdate);
        }
    }
}

