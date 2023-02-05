using System;
using System.Threading.Tasks;
using SearchForApi.Models.Entities;

namespace SearchForApi.Services
{
	public interface IAppService
	{
		Task<(AppRelease newRelease, bool forceUpdate)> CheckForNewRelease(Version version, PlatformType platformType);
	}
}

