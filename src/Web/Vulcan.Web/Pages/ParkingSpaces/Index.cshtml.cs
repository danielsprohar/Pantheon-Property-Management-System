using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nubles.Core.Application.Dto.Reads;
using Nubles.Core.Application.Parameters;
using Vulcan.Web.Services;

namespace Vulcan.Web.Pages.ParkingSpaces
{
    public class IndexModel : PageModel
    {
        private readonly IParkingSpaceService _parkingSpaceService;

        public IndexModel(IParkingSpaceService parkingSpaceService)
        {
            _parkingSpaceService = parkingSpaceService ?? throw new ArgumentNullException(nameof(parkingSpaceService));
        }

        public IEnumerable<ParkingSpaceDto> ParkingSpaces { get; set; }

        public async Task OnGetAsync()
        {
            var parameters = new ParkingSpaceQueryParameters();

            ParkingSpaces = await _parkingSpaceService.GetParkingSpaces(parameters);
        }
    }
}
