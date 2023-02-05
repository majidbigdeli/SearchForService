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
    public class BannedKeywordRepository : BaseRepository<BannedKeyword, Guid>
    {
        public BannedKeywordRepository(ApiContext context) : base(context)
        {
        }

        [Time("id={id}")]
        public override async Task<BannedKeyword> Get(Guid id)
        {
            return await _entities
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        [Time("langauge={langauge}")]
        public async Task<List<BannedKeyword>> GetByLanguage(SceneLangaugeType langauge)
        {
            return await _entities
                .Where(p => p.Language == langauge)
                .ToListAsync();
        }
    }
}