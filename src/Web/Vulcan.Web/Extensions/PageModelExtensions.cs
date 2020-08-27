using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pantheon.Core.Application.Parameters;
using Pantheon.Core.Application.Wrappers;
using System;
using Vulcan.Web.Constants;

namespace Vulcan.Web.Extensions
{
    public static class PageModelExtensions
    {
        /// <summary>
        /// Generates a relative uri that points to the paginated resource list
        ///     located at the given page index.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="page"></param>
        /// <param name="queryParameters"></param>
        /// <param name="pageIndex">The page index</param>
        /// <returns></returns>
        public static string GenerateResourceUri<T>(
            this PageModel page,
            T queryParameters,
            int pageIndex) where T : QueryParameters
        {
            var displayName = page.Url.ActionContext.ActionDescriptor.DisplayName;
            var pageName = displayName.Substring(displayName.LastIndexOf("/") + 1);

            var queryParams = Activator.CreateInstance(typeof(T), new object[] { queryParameters });
            var prop = queryParams.GetType().GetProperty(nameof(QueryParameters.PageIndex));
            prop.SetValue(queryParams, pageIndex);

            return string.Concat(AppConstants.BaseAddress, page.Url.Page(pageName, queryParams));
        }

        public static IActionResult HandleUnsuccessfulApiRequest(this PageModel page, ApiResponse response)
        {
            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.BadRequest:
                    break;

                case System.Net.HttpStatusCode.Unauthorized:
                    // TODO: check this
                    return page.RedirectToRoute(AppConstants.AuthorityLoginAddress);

                case System.Net.HttpStatusCode.Forbidden:
                    break;

                case System.Net.HttpStatusCode.NotFound:
                    break;

                case System.Net.HttpStatusCode.MethodNotAllowed:
                    break;

                case System.Net.HttpStatusCode.NotAcceptable:
                    break;

                case System.Net.HttpStatusCode.ProxyAuthenticationRequired:
                    break;

                case System.Net.HttpStatusCode.RequestTimeout:
                    break;

                case System.Net.HttpStatusCode.UnsupportedMediaType:
                    break;

                case System.Net.HttpStatusCode.InternalServerError:
                    break;

                case System.Net.HttpStatusCode.BadGateway:
                    break;

                case System.Net.HttpStatusCode.GatewayTimeout:
                    break;

                default:
                    return page.Page();
            }

            return page.Page();
        }
    }
}