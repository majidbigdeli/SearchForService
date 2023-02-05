using System;
using System.Threading.Tasks;
using MethodTimer;
using Microsoft.EntityFrameworkCore;
using SearchForApi.Models.DatabaseContext;
using SearchForApi.Models.Entities;

namespace SearchForApi.Repositories
{
	public class UserDeviceRepository : BaseRepository<UserDevice, Guid>
    {
        public UserDeviceRepository(ApiContext context) : base(context)
        {
        }

        [Time("id={id}")]
        public override async Task<UserDevice> Get(Guid id)
        {
            return await _entities
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        [Time("userId={userId},androidId={androidId}")]
        public async Task<UserDevice> GetUserDeviceByAndroidId(Guid userId, string androidId)
        {
            return await _entities
                .FirstOrDefaultAsync(p => p.UserId == userId && p.DeviceId == androidId);
        }
    }
}

