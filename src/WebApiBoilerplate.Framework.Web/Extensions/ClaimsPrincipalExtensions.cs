using System;
using System.Security.Claims;
using JetBrains.Annotations;

namespace WebApiBoilerplate.Framework.Web.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static long? GetUserId([NotNull] this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            var name = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (long.TryParse(name, out long id))
            {
                return id;
            }

            return null;
        }
    }
}
