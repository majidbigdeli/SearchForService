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
    public class UserRepository
    {
        private readonly ApiContext _context;
        public DbSet<User> _entities;

        public UserRepository(ApiContext context)
        {
            _context = context;
            _entities = context.Set<User>();
        }

        [Time("id={id}")]
        public async Task<User> Get(Guid id)
        {
            return await _entities
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        [Time]
        public async Task<List<HistoryMetric>> GetUserMetrics(DateTime startDate)
        {
            var result = await _entities
                .Where(p => p.CreatedOn >= startDate)
                .GroupBy(p => new { p.RegisterMethod, p.RegisterPlatform })
                .Select(p => new
                {
                    p.Key.RegisterMethod,
                    p.Key.RegisterPlatform,
                    Count = p.LongCount(),
                })
                .ToListAsync();

            var normalizedResult = result.Select(p => new HistoryMetric
            {
                Type = HistoryMetricType.Register,
                Count = p.Count,
                Properties = new()
                {
                    { nameof(p.RegisterPlatform), p.RegisterPlatform },
                    { nameof(p.RegisterMethod), p.RegisterMethod }
                }
            }).ToList();

            var normalizedTotalResult = new HistoryMetric
            {
                Type = HistoryMetricType.Register,
                Count = result.Sum(p => p.Count),
                IsTotal = true,
                Properties = new()
            };
            normalizedResult.Add(normalizedTotalResult);

            return normalizedResult;
        }

        [Time]
        public async Task<List<HistoryMetric>> GetUserPlanMetrics(DateTime startDate)
        {
            var result = await _entities
                .Where(p => p.PlanChangedOn >= startDate)
                .GroupBy(p => p.PlanId)
                .Select(p => new
                {
                    PlanId = p.Key,
                    Count = p.LongCount(),
                })
                .ToListAsync();

            var normalizedResult = result.Select(p => new HistoryMetric
            {
                Type = HistoryMetricType.ActivatePlan,
                Count = p.Count,
                Properties = new()
                {
                    { nameof(p.PlanId), p.PlanId },
                }
            }).ToList();

            var normalizedTotalResult = new HistoryMetric
            {
                Type = HistoryMetricType.ActivatePlan,
                Count = result.Sum(p => p.Count),
                IsTotal = true,
                Properties = new()
            };
            normalizedResult.Add(normalizedTotalResult);

            return normalizedResult;
        }
    }
}

