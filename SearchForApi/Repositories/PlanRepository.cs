using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MethodTimer;
using Microsoft.EntityFrameworkCore;
using SearchForApi.Models.DatabaseContext;
using SearchForApi.Models.Entities;

namespace SearchForApi.Repositories
{
    public class PlanRepository : BaseRepository<Plan, PlanType>
    {
        public PlanRepository(ApiContext context) : base(context)
        {
        }

        [Time("id={id}")]
        public override async Task<Plan> Get(PlanType id)
        {
            return await _entities
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        [Time]
        public async Task<List<Plan>> GetAll()
        {
            return await _entities
                .Where(p => p.IsEnable)
                .OrderBy(p => p.Id)
                .ToListAsync();
        }
    }
}

