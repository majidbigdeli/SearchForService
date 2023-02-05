using System;
using SearchForApi.Core;
using SearchForApi.Models.Entities;

namespace SearchForApi.Factories
{
    public class PlanFactory : IPlanFactory
    {
        public int CalculatePlanFinalAmount(Plan plan, bool is3Months, Discount discount = null)
        {
            var finalAmount =
                    !is3Months ? plan.FinalPrice :
                    plan.Price3Months;

            if (discount != null)
                finalAmount = finalAmount - discount.FixedAmount;

            finalAmount = Cfg.IsDev ? 10000 : finalAmount;

            return Math.Max(10000, finalAmount);
        }
    }
}