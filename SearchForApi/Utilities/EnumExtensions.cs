using System;
using System.Collections.Generic;
using System.Linq;

namespace SearchForApi.Utilities
{
    public static class EnumExtensions
    {
        public static T ParseEnum<T>(this string value)
        {
            try
            {
                return (T)Enum.Parse(typeof(T), value, true);
            }
            catch
            {
                return default(T);
            }
        }

        public static T ParseEnum<Q, T>(this Q value)
        {
            try
            {
                return (T)Enum.Parse(typeof(T), value.ToString(), true);
            }
            catch
            {
                return default(T);
            }
        }

        public static IEnumerable<T> GetValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
    }
}