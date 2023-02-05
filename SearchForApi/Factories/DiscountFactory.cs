using System;
using SearchForApi.Models.Entities;

namespace SearchForApi.Factories
{
	public class DiscountFactory : IDiscountFactory
    {
        private readonly IDateTimeFactory _dateTimeFactory;

        public DiscountFactory(IDateTimeFactory dateTimeFactory)
		{
            _dateTimeFactory = dateTimeFactory;
        }

        public bool DiscountIsValid(Discount discount)
        {
            if (discount == null || !discount.IsEnable)
                return false;

            if (discount.StartTime > _dateTimeFactory.Now || discount.EndTime < _dateTimeFactory.Now)
                return false;

            return true;
        }
    }
}

