using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Pantheon.Core.Application.Dto.Reads;
using Pantheon.Core.Application.Parameters;
using Pantheon.Core.Application.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

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

            var response = await _service.GetPaginatedList(parameters);

            if (!response.Succeeded)
            {
                // TODO: handle unsuccessful response
                _logger.LogError(response.Message);

                foreach (var error in response.Errors)
                {
                    _logger.LogError(error);
                }
            }

            ParkingSpaces = response.Data;
        }
    }
}