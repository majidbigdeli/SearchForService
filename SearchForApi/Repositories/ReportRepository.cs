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
    public class ReportRepository : BaseRepository<Report, Guid>
    {
        public ReportRepository(ApiContext context) : base(context)
        {
        }

        [Time("id={id}")]
        public override async Task<Report> Get(Guid id)
        {
            return await _entities
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        [Time]
        public async Task<List<HistoryMetric>> GetReportMetrics(DateTime startDate)
        {
            var result = await _entities
                .Where(p => p.CreatedOn >= startDate)
                .GroupBy(p => p.Type)
                .Select(p => new
                {
                    Type = p.Key,
                    Count = p.LongCount(),
                })
                .ToListAsync();

            var normalizedResult = result.Select(p => new HistoryMetric
            {
                Type = HistoryMetricType.Report,
                Count = p.Count,
                Properties = new()
                {
                    { nameof(p.Type), p.Type },
                }
            }).ToList();

            var normalizedTotalResult = new HistoryMetric
            {
                Type = HistoryMetricType.Report,
                Count = result.Sum(p => p.Count),
                IsTotal = true,
                Properties = new()
            };
            normalizedResult.Add(normalizedTotalResult);

            return normalizedResult;
        }
    }
}

