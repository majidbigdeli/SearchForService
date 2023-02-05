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
    public class ShareRepository : BaseRepository<Share, Guid>
    {
        public ShareRepository(ApiContext context) : base(context)
        {
        }

        [Time("id={id}")]
        public override async Task<Share> Get(Guid id)
        {
            return await _entities
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        [Time("token={token}")]
        public async Task<Share> GetByTokenWithSceneAndMovie(string token)
        {
            return await _entities
                .Include(p => p.Scene)
                .ThenInclude(p => p.Movie)
                .FirstOrDefaultAsync(p => p.Token == token);
        }

        [Time("userId={userId}")]
        public async Task<int> GetShareCountByUserId(Guid userId)
        {
            return await _entities
                .CountAsync(p =>
                    p.UserId == userId);
        }
    }
}