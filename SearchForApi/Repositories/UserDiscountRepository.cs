using System;
using MethodTimer;
using Microsoft.EntityFrameworkCore;
using SearchForApi.Models.DatabaseContext;
using SearchForApi.Models.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace SearchForApi.Repositories
{
	public class UserDiscountRepository : BaseRepository<UserDiscount, Guid>
    {
        public UserDiscountRepository(ApiContext context) : base(context)
        {
        }

        [Time("id={id}")]
        public override async Task<UserDiscount> Get(Guid id)
        {
            return await _entities
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        [Time("userId={userId},discountCode={discountCode}")]
        public async Task<int> GetUsedUserDiscountsByCode(Guid userId, string discountCode)
        {
            return await _entities
                .CountAsync(p => p.UserId == userId && p.DiscountCode == discountCode);
        }
    }
}

