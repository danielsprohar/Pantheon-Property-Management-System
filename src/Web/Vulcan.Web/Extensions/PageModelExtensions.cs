using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pantheon.Core.Application.Wrappers;
using Vulcan.Web.Constants;

namespace Vulcan.Web.Extensions
{
    public static class PageModelExtensions
    {
        public static IActionResult HandleUnsuccessfulApiRequest(this PageModel pageModel, ApiResponse response)
        {
            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.BadRequest:
                    break;

                case System.Net.HttpStatusCode.Unauthorized:
                    // TODO: check this
                    return pageModel.RedirectToRoute(AppConstants.AuthorityLoginAddress);

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
                    return pageModel.Page();
            }

            return pageModel.Page();
        }
    }
}