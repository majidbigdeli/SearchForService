using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MethodTimer;
using Microsoft.EntityFrameworkCore;
using SearchForApi.Integrations.Payment;
using SearchForApi.Models.DatabaseContext;
using SearchForApi.Models.Entities;

namespace SearchForApi.Repositories
{
    public class PaymentGatewayRepository : BaseRepository<PaymentGateway, PaymentGatewayType>
    {
        public PaymentGatewayRepository(ApiContext context) : base(context)
        {
        }

        [Time("id={id}")]
        public override async Task<PaymentGateway> Get(PaymentGatewayType id)
        {
            return await _entities
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        [Time]
        public async Task<List<PaymentGateway>> GetActives()
        {
            return await _entities
                .Where(p => p.IsEnable)
                .OrderBy(p => p.Order)
                .ToListAsync();
        }
    }
}
