using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SearchForApi.Integrations.Payment;
using SearchForApi.Models;
using SearchForApi.Models.Entities;

namespace SearchForApi.Services
{
    public interface IPlanService
    {
        Task<Discount> GetDiscount(Guid userId, string discountCode);
        Task<(RequestTokenDto requestToken, bool needToPay)> RequestPlan(Guid userId, PlanType planId, bool is3Months, string redirectUrl, string discountCode);
        Task<(bool succeeded, string redirectUrl)> VerifyPlanPayment(string refIdString, IFormCollection formData, IQueryCollection queryString);
        Task IsAllowedToSearch(Guid userId, string keyword, int sceneIndex);
        Task IsAllowedToBookmark(Guid userId);
        Task IsAllowedToShare(Guid userId);
        Task UpdateUserStatus(Guid userId, UserPlanStatusType type, bool incremental, string keyword = null);
        Task IsAllowedViewPredefinedItemScene(Guid? userId, int sceneIndex);
    }
}