using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Vulcan.Web.Constants;

namespace Vulcan.Web.Pages
{
    public class LogoutModel : PageModel
    {
        private readonly ILogger<LogoutModel> _logger;

        public LogoutModel(ILogger<LogoutModel> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync();
            }

            return SignOut(AppConstants.CookieScheme, "oidc");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await HttpContext.SignOutAsync();
            _logger.LogInformation("User logged out");
            return SignOut(AppConstants.CookieScheme, "oidc");

            //var encodedUrl = UrlEncoder.Create(new TextEncoderSettings()).Encode(_returnUrl);
            //return Redirect($"https://localhost:6001/Identity/Account/Logout?returnUrl={encodedUrl}");
        }
    }
}