using Microsoft.AspNetCore.Mvc.RazorPages;
using Nubles.Core.Application.Parameters;
using Pantheon.Core.Application.Dto.Reads;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vulcan.Web.Services;

namespace Vulcan.Web.Pages.ParkingSpaces
{
    public class IndexModel : PageModel
    {
        private readonly IParkingSpaceService _service;

        public IndexModel(IParkingSpaceService service)
        {
            _service = service;
        }

        public IEnumerable<ParkingSpaceDto> ParkingSpaces { get; set; }

        public async Task OnGetAsync()
        {
            // TODO: move this
            var parameters = new ParkingSpaceQueryParameters();

            ParkingSpaces = await _service.GetParkingSpaces(parameters);
        }
    }
}