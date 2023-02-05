using SearchForApi.Models.Entities;

namespace SearchForApi.Factories
{
    public interface IPlanFactory
    {
        int CalculatePlanFinalAmount(Plan plan, bool is3Months, Discount discount = null);
    }
}