using System;

namespace SearchForApi.Factories
{
    public interface IDateTimeFactory
    {
        DateTime Now { get; }
        DateTime UtcNow { get; }

        DateTime AddPersianMonths(DateTime dateTime, int months);
    }
}

