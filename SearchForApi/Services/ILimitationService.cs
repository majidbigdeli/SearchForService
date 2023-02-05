using System;
using System.Threading.Tasks;
using SearchForApi.Models;
using SearchForApi.Models.Dtos;
using SearchForApi.Models.Entities;

namespace SearchForApi.Services
{
    public interface ILimitationService
    {
        Task<(long total, MovieAssetDto result)> GetUncheckedMovieAsset();
        Task CheckdMovieAsset(Guid userId, Guid movieId, AssetsCheckStatus status);
        Task Check(Guid userId, Guid sceneId, SceneCheckType type, SceneCheckResultType resultType, SceneCheckExcludeType? excludeType);
    }
}
