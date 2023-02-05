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
    public class BookmarkRepository : BaseRepository<Bookmark, Guid>
    {
        public BookmarkRepository(ApiContext context) : base(context)
        {
        }

        [Time("id={id}")]
        public override async Task<Bookmark> Get(Guid id)
        {
            return await _entities
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        [Time("userId={userId},sceneId={sceneId}")]
        public async Task<Bookmark> GetByUserIdAndSceneId(Guid userId, Guid sceneId)
        {
            return await _entities
                .FirstOrDefaultAsync(p =>
                    p.UserId == userId &&
                    p.SceneId == sceneId);
        }

        [Time("userId={userId},sceneId={sceneId}")]
        public async Task<bool> IsUserBookemarkedScene(Guid userId, Guid sceneId)
        {
            return await _entities
                .AnyAsync(p =>
                    p.UserId == userId &&
                    p.SceneId == sceneId);
        }

        [Time("userId={userId}")]
        public async Task<int> GetCountByUserId(Guid userId)
        {
            return await _entities
                .CountAsync(p =>
                    p.UserId == userId);
        }

        [Time("userId={userId},skip={skip},take={take}")]
        public async Task<(long total, List<Bookmark>)> GetUserCheckdBookmarks(Guid userId, int skip, int take)
        {
            var total = await _entities
                .Include(i => i.Scene)
                .CountAsync(p => p.UserId == userId && p.Scene.CheckResultType != SceneCheckResultType.Excluded);

            var items = await _entities
                .Include(i => i.Scene)
                .ThenInclude(i => i.Movie)
                .Where(p => p.UserId == userId && p.Scene.CheckResultType != SceneCheckResultType.Excluded)
                .OrderByDescending(q => q.CreatedOn)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return (total, items);
        }

        [Time]
        public async Task<List<HistoryMetric>> GetBookmarkMetrics(DateTime startDate)
        {
            var result = await _entities
                .CountAsync(p => p.CreatedOn >= startDate);

            var normalizedResult = new HistoryMetric
            {
                Type = HistoryMetricType.Bookmark,
                Count = result,
                IsTotal = true,
                Properties = new()
            };

            return new() { normalizedResult };
        }
    }
}