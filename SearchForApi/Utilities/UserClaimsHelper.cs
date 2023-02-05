using System;
using System.Linq;
using System.Security.Claims;

namespace SearchForApi.Utilities
{
    public static class UserClaimsHelper
    {
        public static Guid Id(this ClaimsPrincipal claimsPrincipal)
        {
            var claim = claimsPrincipal.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault();

            if (claim == null)
                throw new UnauthorizedAccessException("Jwt nameId claim not found");

            return Guid.Parse(claim.Value);
        }

        public static Guid? IdOrDefault(this ClaimsPrincipal claimsPrincipal)
        {
            var claim = claimsPrincipal.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault();

            _ = Guid.TryParse(claim?.Value, out Guid userId);

            return userId == Guid.Empty ? null : userId;
        }

        public static string Username(this ClaimsPrincipal claimsPrincipal)
        {
            var claim = claimsPrincipal.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault();

            if (claim == null)
                throw new UnauthorizedAccessException("Jwt username claim not found");

            return claim.Value;
        }
    }
}

