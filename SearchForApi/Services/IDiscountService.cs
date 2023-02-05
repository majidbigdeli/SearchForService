using System;
using System.Threading.Tasks;
using SearchForApi.Models.Entities;

namespace SearchForApi.Services
{
    public interface IDiscountService
    {
        Task<Discount> GetDisountCode(Guid userId, string discountCode);
        Task<UserDiscount> AddNewUserDiscount(Guid userId, Discount discount);
        Task<UserDiscount> AddNewUserDiscount(Guid userId, string discountCode);
    }
}