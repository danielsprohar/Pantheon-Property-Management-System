using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Pantheon.Core.Application.Dto.Reads;
using Pantheon.Core.Application.Services;
using System.Threading.Tasks;
using Vulcan.Web.Extensions;

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
            var response = await _service.Get(id);

            if (!response.Succeeded)
            {
                // TODO: handle error
                _logger.LogError($"Status: {response.StatusCode}; Message: {response.Message}");

                foreach (var error in response.Errors)
                {
                    _logger.LogError(error);
                }

                this.HandleUnsuccessfulApiRequest(response);
            }

            ParkingSpace = response.Data;
        }
    }
}