using System;
using SearchForApi.Integrations.Payment;
using System.Threading.Tasks;
using SearchForApi.Models.Entities;

namespace SearchForApi.Services
{
	public interface IRequestPlanService
	{
        Task RequestFreePlan(Guid userId, PlanType planId, bool is3Months, Discount discount);
        Task<RequestTokenDto> RequestPaidPlan(Guid userId, Plan plan, bool is3Months, string redirectUrl, Discount discount);
    }
}

