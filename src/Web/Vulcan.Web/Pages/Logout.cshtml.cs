using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Vulcan.Web.Constants;

namespace Vulcan.Web.Pages
{
    public class LogoutModel : PageModel
    {
        private readonly ILogger<LogoutModel> _logger;
        private readonly string _returnUrl = "https://localhost:5002";

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

        //public IActionResult OnPost()
        //{
        //    return SignOut(AppConstants.CookieScheme, "oidc");
        //    //await HttpContext.SignOutAsync();
        //    //_logger.LogInformation("User logged out");

        //    //var encodedUrl = UrlEncoder.Create(new TextEncoderSettings()).Encode(_returnUrl);

        //    //return Redirect($"https://localhost:6001/Identity/Account/Logout?returnUrl={encodedUrl}");
        //}
    }
}