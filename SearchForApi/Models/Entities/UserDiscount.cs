using System;
namespace SearchForApi.Models.Entities
{
	public class UserDiscount : BaseEntity<Guid>
    {
        public DateTime CreatedOn { get; set; }
        public Guid UserId { get; set; }
        public string DiscountCode { get; set; }

        public int? DiscountFixedAmount { get; set; }
        public int? DiscountPercent { get; set; } // This property is just for determining how percent is affected the fixed amount and don't use in any calculations
        public bool IsFree { get; set; }

        public User User { get; set; }
        public Discount Discount { get; set; }
    }
}

