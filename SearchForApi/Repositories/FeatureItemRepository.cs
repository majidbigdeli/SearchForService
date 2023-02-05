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
    public class FeatureItemRepository : BaseRepository<FeatureItem, Guid>
    {
        public FeatureItemRepository(ApiContext context) : base(context)
        {
        }

        [Time("id={id}")]
        public override async Task<FeatureItem> Get(Guid id)
        {
            return await _entities
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        [Time("id={id}")]
        public async Task<FeatureItem> GetWithFeature(Guid id)
        {
            return await _entities
                .Include(p => p.Feature)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        [Time("featureId={featureId}")]
        public async Task<(long total, List<FeatureItem>)> GetByFeatureId(Guid featureId, int skip, int take)
        {
            var total = await _entities
                .Where(p => p.FeatureId == featureId && p.IsEnable)
                .CountAsync();

            var items = await _entities
                .Where(p => p.FeatureId == featureId && p.IsEnable)
                .OrderBy(p => p.Order)
                .ThenByDescending(p => p.CreatedOn)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return (total, items);
        }
    }
}

