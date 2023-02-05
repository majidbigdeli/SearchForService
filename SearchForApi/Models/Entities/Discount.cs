using System;
using System.Collections.Generic;

namespace SearchForApi.Models.Entities
{
    public class Discount : BaseEntity<string>
    {
        public bool IsEnable { get; set; }
        public DateTime StartTime { get; set; } // Local DateTime
        public DateTime EndTime { get; set; } // Local DateTime
        public string Description { get; set; }
        public int FixedAmount { get; set; }
        public int Percent { get; set; } // This property is just for determining how percent is affected the fixed amount and don't use in any calculations
        public bool IsFree { get; set; }
        public int CountPerUser { get; set; }

        public ICollection<Payment> Payments { get; set; }
        public ICollection<UserDiscount> UserDiscounts { get; set; }
    }
}