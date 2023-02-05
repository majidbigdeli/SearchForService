using System;
using SearchForApi.Models.Entities;

namespace SearchForApi.Factories
{
	public class UserDiscountFactory : IUserDiscountFactory
    {
        private readonly IDateTimeFactory _dateTimeFactory;

        public UserDiscountFactory(IDateTimeFactory dateTimeFactory)
        {
            _dateTimeFactory = dateTimeFactory;
        }

        public UserDiscount CreateNewUserDiscountInstance(Guid userId, Discount discount)
        {
            var newUserDiscount = new UserDiscount
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CreatedOn = _dateTimeFactory.UtcNow,
                DiscountCode = discount.Id,
                DiscountFixedAmount = discount.FixedAmount,
                DiscountPercent = discount.Percent,
                IsFree = discount.IsFree
            };

            return newUserDiscount;
        }
    }
}

