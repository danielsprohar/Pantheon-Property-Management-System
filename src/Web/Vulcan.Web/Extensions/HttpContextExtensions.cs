using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Vulcan.Web.Extensions
{
    public static class HttpContextExtensions
    {
        public static async Task<HttpContext> SetBearerToken(this HttpContext httpContext, HttpClient httpClient)
        {
            var accessToken = await httpContext.GetTokenAsync("access_token");

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            
            return httpContext;
        }

        public static string GetUserId(this HttpContext httpContext)
        {
            return httpContext.User.FindFirstValue("sub");
        }
    }
}