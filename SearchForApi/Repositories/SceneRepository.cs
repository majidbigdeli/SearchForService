using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MethodTimer;
using Microsoft.EntityFrameworkCore;
using SearchForApi.Factories;
using SearchForApi.Models.DatabaseContext;
using SearchForApi.Models.Entities;

namespace SearchForApi.Repositories
{
    public class SceneRepository : BaseRepository<Scene, Guid>
    {
        private readonly ISceneFactory _sceneFactory;

        public SceneRepository(ApiContext context, ISceneFactory sceneFactory) : base(context)
        {
            _sceneFactory = sceneFactory;
        }

        [Time("id={id}")]
        public override async Task<Scene> Get(Guid id)
        {
            return await _entities
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        [Time("id={id}")]
        public override async Task<bool> Exist(Guid id)
        {
            return await _entities
                .AnyAsync(p => p.Id == id);
        }

        [Time("id={id}")]
        public async Task<Scene> GetWithMovie(Guid id)
        {
            return await _entities
                .Include(i => i.Movie)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        [Time("movieId={movieId},startTime={startTime},endTime={endTime}")]
        public async Task<List<Scene>> GetCheckedNearItems(Guid movieId, int startTime, int endTime)
        {
            var segmentInfo = _sceneFactory.CalculateSceneTimes(startTime, endTime);

            var result = await _entities
                    .Where(p =>
                        p.MovieId == movieId &&
                        p.StartTime <= segmentInfo.ActualEndSegmentTime &&
                        p.EndTime >= segmentInfo.ActualStartSegmentTime &&
                        p.CheckResultType != SceneCheckResultType.Excluded)
                    .OrderBy(p => p.StartTime)
                    .ToListAsync();

            return result;
        }

        [Time("movieId={movieId},startTime={startTime},endTime={endTime}")]
        public async Task<List<Scene>> GetNearItems(Guid movieId, int startTime, int endTime)
        {
            var segmentInfo = _sceneFactory.CalculateSceneTimes(startTime, endTime);

            var result = await _entities
                    .Where(p =>
                        p.MovieId == movieId &&
                        p.StartTime <= segmentInfo.ActualEndSegmentTime &&
                        p.EndTime >= segmentInfo.ActualStartSegmentTime)
                    .OrderBy(p => p.StartTime)
                    .ToListAsync();

            return result;
        }
    }
}

