using SearchForApi.Models.Entities;

namespace SearchForApi.Models.Dtos
{
    public class PlansDto
    {
        public PlanType Id { get; set; }
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
    }
}

