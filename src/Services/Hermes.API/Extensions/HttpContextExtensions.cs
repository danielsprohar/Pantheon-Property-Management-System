using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace Hermes.API.Extensions
{
    public static class HttpContextExtensions
    {
        /// <summary>
        /// Searchs the user's claims for the <c>sub</c> claim,
        /// then constructs a new <c>Guid</c> from the claim value.
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns>
        /// Returns the user id;
        /// if a user is not logged in, then the <c>default</c> value of <c>Guid</c> is returned.
        /// </returns>
        public static Guid GetUserId(this HttpContext httpContext)
        {
            if (httpContext.User == null)
            {
                return default;
            }

            var identity = httpContext.User.Identity as ClaimsIdentity;

            if (identity == null)
            {
                return default;
            }

            Claim claim = identity.FindFirst(ClaimTypes.NameIdentifier);

            return new Guid(claim.Value);
        }
    }
}