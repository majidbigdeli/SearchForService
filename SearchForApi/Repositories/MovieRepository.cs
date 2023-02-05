using System;
using System.Linq;
using System.Threading.Tasks;
using MethodTimer;
using Microsoft.EntityFrameworkCore;
using SearchForApi.Models.DatabaseContext;
using SearchForApi.Models.Entities;

namespace SearchForApi.Repositories
{
    public class MovieRepository : BaseRepository<Movie, Guid>
    {
        public MovieRepository(ApiContext context) : base(context)
        {
        }

        [Time("id={id}")]
        public override async Task<Movie> Get(Guid id)
        {
            return await _entities
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        [Time]
        public async Task<(long total, Movie movie)> GetAssetsUncheckedMovie()
        {
            var total = await _entities
                .Where(p => p.AssetsCheckedStatus == AssetsCheckStatus.None)
                .CountAsync();

            var result = await _entities
                .Where(p => p.AssetsCheckedStatus == AssetsCheckStatus.None)
                .OrderByDescending(p => p.InternalId)
                .Take(1)
                .FirstOrDefaultAsync();

            return (total, result);
        }
    }
}

