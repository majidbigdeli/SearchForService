using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MethodTimer;
using Microsoft.EntityFrameworkCore;
using SearchForApi.Models.DatabaseContext;
using SearchForApi.Models.Entities;

namespace SearchForApi.Repositories
{
    public class ShareHistoryRepository : BaseRepository<ShareHistory, Guid>
    {
        public ShareHistoryRepository(ApiContext context) : base(context)
        {
        }

        [Time("id={id}")]
        public override async Task<ShareHistory> Get(Guid id)
        {
            return await _entities
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        [Time]
        public async Task<List<HistoryMetric>> GetShareHistoryMetrics(DateTime startDate)
        {
            var total = await _entities
                .CountAsync(p => p.CreatedOn >= startDate);

            var normalizedResult = new HistoryMetric
            {
                Type = HistoryMetricType.Share,
                Count = total,
                IsTotal = true,
                Properties = new()
            };

            return new() { normalizedResult };
        }
    }
}
