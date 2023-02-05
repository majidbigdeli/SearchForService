using System;
using SearchForApi.Models.Entities;

namespace SearchForApi.Factories
{
	public interface IDiscountFactory
	{
        bool DiscountIsValid(Discount discount);
    }
}

