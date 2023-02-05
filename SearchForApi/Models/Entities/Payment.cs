using System;
using SearchForApi.Integrations.Payment;

namespace SearchForApi.Models.Entities
{
    public class Payment : BaseEntity<Guid>
    {
        public DateTime CreatedOn { get; set; }
        public Guid UserId { get; set; }
        public PaymentStatus Status { get; set; }
        public DateTime StatusChangedOn { get; set; }
        public string StatusMessage { get; set; }
        public int Amount { get; set; }
        public string Description { get; set; }
        public string RedirectUrl { get; set; }

        public PlanType PlanId { get; set; }
        public int PlanPrice { get; set; }
        public bool PlanIs3Months { get; set; }
        public int PlanDiscountPrecentFor3Months { get; set; }
        public int PlanDiscount { get; set; }
        public int Price3Months { get; set; }

        public string DiscountCode { get; set; }

        public PaymentGatewayType? GatewayType { get; set; }
        public string Token { get; set; }
        public string PayUrl { get; set; }

        public string CallbackStatusCode { get; set; }
        public string CardNumber { get; set; }
        public string CardHashPan { get; set; }

        public Plan Plan { get; set; }
        public User User { get; set; }
        public Discount Discount { get; set; }

        public PaymentGateway Gateway { get; set; }
    }

    public enum PaymentStatus
    {
        New = 1,
        Token,
        Paid,
        Verified,
        FailedByUser,
        FailedByToken,
        FailedByVerify,
    }
}