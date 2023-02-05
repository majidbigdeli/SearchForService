using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MethodTimer;
using Microsoft.EntityFrameworkCore;
using SearchForApi.Factories;
using SearchForApi.Models.DatabaseContext;
using SearchForApi.Models.Entities;

namespace SearchForApi.Repositories
{
    public class DiscountRepository : BaseRepository<Discount, string>
    {
        private readonly IDateTimeFactory _dateTimeFactory;

        public DiscountRepository(ApiContext context, IDateTimeFactory dateTimeFactory) : base(context)
        {
            _dateTimeFactory = dateTimeFactory;
        }

        [Time("id={id}")]
        public override async Task<Discount> Get(string id)
        {
            return await _entities
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        [Time]
        public async Task<List<string>> GetCodes()
        {
            return await _entities
                .Where(p => p.IsEnable && p.EndTime > _dateTimeFactory.UtcNow)
                .Select(p => p.Id).OrderBy(q => q).ToListAsync();
        }
    }
}