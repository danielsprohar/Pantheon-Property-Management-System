using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Pantheon.Core.Application.Dto.Reads;
using Pantheon.Core.Application.Parameters;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vulcan.Web.Services;

namespace Vulcan.Web.Pages.ParkingSpaces
{
    public class IndexModel : PageModel
    {
        private readonly IParkingSpaceService _service;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(IParkingSpaceService service, ILogger<IndexModel> logger)
        {
            _service = service;
            _logger = logger;
        }

        public IEnumerable<ParkingSpaceDto> ParkingSpaces { get; set; }

        public async Task OnGetAsync()
        {
            // TODO: Implement the UI
            var parameters = new ParkingSpaceQueryParameters();

            var paginatedApiResponse = await _service.GetParkingSpacesAsync(parameters);

            if (!paginatedApiResponse.Succeeded)
            {
                _logger.LogError(paginatedApiResponse.Message);

                foreach (var error in paginatedApiResponse.Errors)
                {
                    _logger.LogError(error);
                }
            }

            ParkingSpaces = paginatedApiResponse.Data;
        }
    }
}