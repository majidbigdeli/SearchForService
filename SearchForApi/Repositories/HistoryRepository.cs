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
    public class HistoryRepository : BaseRepository<History, Guid>
    {
        private readonly ApiContext _context;

        public HistoryRepository(ApiContext context) : base(context)
        {
            _context = context;
        }

        [Time("id={id}")]
        public override async Task<History> Get(Guid id)
        {
            return await _entities
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        [Time("userId={userId},skip={skip},take={take}")]
        public async Task<(long total, List<History>)> GetUserSearchHistories(Guid userId, DateTime startDate, int skip, int take)
        {
            var total = await _entities
            .Where(p =>
                p.UserId == userId &&
                p.CreatedOn >= startDate &&
                p.Type == HistoryType.Search &&
                p.SearchIsFound)
                .GroupBy(p => p.SearchKeyword)
                .CountAsync();

            var items = await _entities
            .Where(p =>
                p.UserId == userId &&
                p.CreatedOn >= startDate &&
                p.Type == HistoryType.Search &&
                p.SearchIsFound)
                .GroupBy(p => p.SearchKeyword)
                .Select(p => new
                {
                    SearchKeyword = p.Key,
                    CreatedOn = p.Max(q => q.CreatedOn),
                    Id = p.Max(q => q.Id.ToString())
                })
                .OrderByDescending(l => l.CreatedOn)
            .Skip(skip)
            .Take(take)
            .ToListAsync();

            var normalizedItems = items.Select(p => new History
            {
                Id = Guid.Parse(p.Id),
                SearchKeyword = p.SearchKeyword,
                CreatedOn = p.CreatedOn
            }).ToList();

            return (total, normalizedItems);
        }

        [Time]
        public async Task<List<HistoryMetric>> GetSearchMetrics(DateTime startDate)
        {
            var result = await _entities
                .Where(p => p.Type == HistoryType.Search && p.CreatedOn >= startDate)
                .GroupBy(p => p.SearchIsFound)
                .Select(p => new
                {
                    SearchIsFound = p.Key,
                    Count = p.LongCount(),
                })
                .ToListAsync();

            var normalizedResult = result.Select(p => new HistoryMetric
            {
                Type = HistoryMetricType.Search,
                Count = p.Count,
                Properties = new()
                {
                    { nameof(p.SearchIsFound), p.SearchIsFound }
                }
            }).ToList();

            var normalizedTotalResult = new HistoryMetric
            {
                Type = HistoryMetricType.Search,
                Count = result.Sum(p => p.Count),
                IsTotal = true,
                Properties = new()
            };
            normalizedResult.Add(normalizedTotalResult);

            return normalizedResult;
        }

        [Time]
        public async Task<List<HistoryMetric>> GetSceneMetrics(DateTime startDate)
        {
            var total = await _entities
                .CountAsync(p => p.Type == HistoryType.Scene && p.CreatedOn >= startDate);

            var normalizedResult = new HistoryMetric
            {
                Type = HistoryMetricType.Scene,
                Count = total,
                IsTotal = true
            };

            return new() { normalizedResult };
        }

        [Time]
        public async Task<List<HistoryMetric>> GetSearchReferMetrics(DateTime startDate)
        {
            var result = await _entities
                .Where(p => p.Type == HistoryType.Search && p.SearchIsFound && p.CreatedOn >= startDate)
                .GroupBy(p => p.ReferType)
                .Select(p => new
                {
                    ReferType = p.Key,
                    Count = p.LongCount(),
                })
                .ToListAsync();

            var normalizedResult = result.Select(p => new HistoryMetric
            {
                Type = HistoryMetricType.SearchRefer,
                Count = p.Count,
                Properties = new()
                {
                    { nameof(p.ReferType), p.ReferType }
                }
            }).ToList();

            var normalizedTotalResult = new HistoryMetric
            {
                Type = HistoryMetricType.SearchRefer,
                Count = result.Sum(p => p.Count),
                IsTotal = true,
                Properties = new()
            };
            normalizedResult.Add(normalizedTotalResult);

            return normalizedResult;
        }

        [Time("take={take}")]
        public async Task<Dictionary<string, string>> GetKeywordsRandomly(int take = 10)
        {
            var result = await _entities
                    .Where(p => p.SearchIsFound &&
                        p.Type == HistoryType.Search &&
                        p.SearchKeyword.Length < 20 &&
                        !_context.BannedKeywords.Any(q => q.Keyword == p.SearchKeyword.ToLower()))
                    .GroupBy(q => q.SearchKeyword)
                    .Select(p => new { Id = p.Min(q => q.Id.ToString()), p.Key, SortId = Guid.NewGuid() })
                    .OrderBy(p => p.SortId) // This functionality needs to "uuid-ossp" extension installed on the database
                    .Take(take)
                    .ToDictionaryAsync(p => p.Id, p => p.Key);

            return result;
        }
    }
}

