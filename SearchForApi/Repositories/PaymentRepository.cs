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
    public class PaymentRepository : BaseRepository<Payment, Guid>
    {
        private readonly DiscountRepository _discountRepository;

        public PaymentRepository(ApiContext context, DiscountRepository discountRepository) : base(context)
        {
            _discountRepository = discountRepository;
        }

        [Time("id={id}")]
        public override async Task<Payment> Get(Guid id)
        {
            return await _entities
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        [Time("id={id}")]
        public async Task<Payment> GetWithGateway(Guid id)
        {
            return await _entities
                .Include(p => p.Gateway)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        [Time]
        public async Task<List<HistoryMetric>> GetPaymentMetrics(DateTime startDate)
        {
            var result = await _entities
                .Where(p => p.StatusChangedOn >= startDate)
                .GroupBy(p => p.Status)
                .Select(p => new
                {
                    Status = p.Key,
                    Count = p.LongCount(),
                })
                .ToListAsync();

            var normalizedResult = result.Select(p => new HistoryMetric
            {
                Type = HistoryMetricType.Payment,
                Count = p.Count,
                Properties = new()
                {
                    { nameof(p.Status), p.Status },
                }
            }).ToList();

            var normalizedTotalResult = new HistoryMetric
            {
                Type = HistoryMetricType.Payment,
                Count = result.Sum(p => p.Count),
                IsTotal = true,
                Properties = new()
            };
            normalizedResult.Add(normalizedTotalResult);

            return normalizedResult;
        }

        [Time]
        public async Task<List<HistoryMetric>> GetIncomeMetrics(DateTime startDate)
        {
            var result = await _entities
                .Where(p => p.Status == PaymentStatus.Verified && p.StatusChangedOn >= startDate)
                .SumAsync(p => p.Amount);

            var normalizedResult = new HistoryMetric
            {
                Type = HistoryMetricType.Income,
                Count = result,
                IsTotal = true,
                Properties = new()
            };

            return new() { normalizedResult };
        }

        [Time]
        public async Task<List<HistoryMetric>> GetDiscountMetrics(DateTime startDate)
        {
            var discounts = await _discountRepository.GetCodes();

            var result = await _entities
                .Where(p => p.Status == PaymentStatus.Verified &&
                    p.DiscountCode != null &&
                    p.StatusChangedOn >= startDate)
                .GroupBy(p => p.DiscountCode)
                .Select(p => new
                {
                    DiscountCode = p.Key,
                    Count = p.LongCount(),
                })
                .ToListAsync();

            var normalizedResult = result.Select(p => new HistoryMetric
            {
                Type = HistoryMetricType.Discount,
                Count = p.Count,
                Properties = new()
                {
                    { nameof(p.DiscountCode), p.DiscountCode },
                }
            }).ToList();

            var normalizedTotalResult = new HistoryMetric
            {
                Type = HistoryMetricType.Discount,
                Count = result.Sum(p => p.Count),
                IsTotal = true,
                Properties = new()
            };
            normalizedResult.Add(normalizedTotalResult);

            return normalizedResult;
        }
    }
}