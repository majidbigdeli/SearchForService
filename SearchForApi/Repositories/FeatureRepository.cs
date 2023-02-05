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
    public class FeatureRepository : BaseRepository<Feature, Guid>
    {
        public FeatureRepository(ApiContext context) : base(context)
        {
        }

        [Time("id={id}")]
        public override async Task<Feature> Get(Guid id)
        {
            return await _entities
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        [Time]
        public async Task<List<Feature>> GetAvailable()
        {
            return await _entities
                .Where(p => p.IsEnable)
                .OrderBy(p => p.Order)
                .ThenByDescending(p => p.CreatedOn)
                .ToListAsync();
        }
    }
}

