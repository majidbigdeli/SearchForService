using System;
using System.Globalization;

namespace SearchForApi.Factories
{
    public class DateTimeFactory : IDateTimeFactory
    {
        public DateTime Now => DateTime.Now;
        public DateTime UtcNow => DateTime.UtcNow;

        public DateTime AddPersianMonths(DateTime dateTime, int months) => new PersianCalendar().AddMonths(dateTime, months);
    }
}