using System;
using System.Collections.Generic;
using SearchForApi.Integrations.Payment;

namespace SearchForApi.Models.Entities
{
    public class PaymentGateway : BaseEntity<PaymentGatewayType>
    {
        public bool IsEnable { get; set; }
        public int Order { get; set; }
        public string AccessToken { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? ModifiedByUserId { get; set; }

        public ICollection<Payment> Payments { get; set; }

        public User ModifiedByUser { get; set; }
    }
}
