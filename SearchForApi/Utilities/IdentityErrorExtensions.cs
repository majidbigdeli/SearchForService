using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace SearchForApi.Utilities
{
    public static class IdentityErrorExtensions
    {
        public static string ToLogString(this IEnumerable<IdentityError> errors)
        {
            var message = string.Join('\n', errors.Select(p => $"{p.Code}-{p.Description}"));
            return message;
        }

        public static bool DuplicateUserName(this IEnumerable<IdentityError> errors)
        {
            return errors.Any(p => p.Code == "DuplicateUserName");
        }
    }
}