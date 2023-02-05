using SearchForApi.Core;
using SearchForApi.Models.Entities;

namespace SearchForApi.Models.Dtos
{
    public class UserProfileDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string AvatarImageUrl { get; set; }
        public PlanType PlanType { get; set; }
        public int? ExpireDays { get; set; }
        public bool ShouldShowRenewPlan => ExpireDays.HasValue && ExpireDays <= Cfg.UserShouldBeRenewPlanDaysBefore;
        public int TrialDaysRemain { get; set; }
        public bool TrialEnded => TrialDaysRemain == 0;
    }
}

