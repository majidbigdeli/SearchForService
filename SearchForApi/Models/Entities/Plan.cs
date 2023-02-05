using System.Collections.Generic;

namespace SearchForApi.Models.Entities
{
    public class Plan: BaseEntity<PlanType>
    {
        public string Title { get; set; }
        public int Price { get; set; }
        public int Discount { get; set; }
        public int DiscountPrecentFor3Months { get; set; }
        public int Price3Months { get; set; }
        public int? SearchPerDay { get; set; }
        public int? QuoteViewPerSearch { get; set; }
        public int? ShareQuote { get; set; }
        public int? BookmarkedCount { get; set; }
        public int? LastHistoriesDays { get; set; }
        public bool KidsMode { get; set; }
        public bool IsEnable { get; set; }

        public ICollection<User> Users { get; set; }
        public ICollection<Payment> Payments { get; set; }

        public int FinalPrice => Price - Discount;
    }

    public enum PlanType
    {
        Basic = 1,
        Pro = 2,
        Kids = 3,
        Trial = 4
    }
}