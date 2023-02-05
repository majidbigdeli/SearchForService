using System;
using SearchForApi.Models.Entities;

namespace SearchForApi.Factories
{
	public interface IUserDiscountFactory
	{
        UserDiscount CreateNewUserDiscountInstance(Guid userId, Discount discount);
    }
}

