using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace SearchForApi.Models.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string Name { get; set; }
        public RegisterMethodType RegisterMethod { get; set; }
        public PlatformType RegisterPlatform { get; set; }
        public PlanType PlanId { get; set; }
        public DateTime PlanChangedOn { get; set; }
        public DateTime? PlanExpireDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public string StorageId { get; set; }

        public PlanType ActualPlanId => PlanExpireDate.HasValue && PlanExpireDate.Value < DateTime.UtcNow ? PlanType.Basic : PlanId;

        public Plan Plan { get; set; }

        public ICollection<UserDevice> Devices { get; set; }
        public ICollection<Report> Reports { get; set; }
        public ICollection<PaymentGateway> ModifiedPaymentGateways { get; set; }
        public ICollection<Movie> AssetsCheckedMovies { get; set; }
        public ICollection<Scene> CheckedScenes { get; set; }
        public ICollection<Payment> Payments { get; set; }
        public ICollection<History> Histories { get; set; }
        public ICollection<Bookmark> Bookmarks { get; set; }
        public ICollection<Share> Shares { get; set; }
        public ICollection<FeatureItem> ChangedFeatureItems { get; set; }
        public ICollection<UserDiscount> UserDiscounts { get; set; }
    }

    public enum RegisterMethodType
    {
        Email,
        Google,
        PhoneNumber
    }

    public enum PlatformType
    {
        None,
        WebApp,
        PWA,
        Android,
        Admin,
        Next,
        NextPwa
    }
}