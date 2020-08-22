using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Pantheon.Core.Application.Dto.Reads;
using System.Threading.Tasks;
using Vulcan.Web.Services;

namespace Vulcan.Web.Pages.ParkingSpaces
{
    public class DetailsModel : PageModel
    {
        private readonly ILogger<DetailsModel> _logger;
        private readonly IParkingSpaceService _service;

        public ParkingSpaceDto ParkingSpace { get; set; }

        public DetailsModel(
            ILogger<DetailsModel> logger,
            IParkingSpaceService service)
        {
            _logger = logger;
            _service = service;
        }

        public async Task OnGetAsync(int id)
        {
            var response = await _service.GetParkingSpaceAsync(id);

            if (response.Succeeded)
            {
                ParkingSpace = response.Data;
            }
            else
            {
                // TODO: show error page
                _logger.LogError(response.Message);

                foreach (var error in response.Errors)
                {
                    _logger.LogError(error);
                }
            }
        }
    }
}