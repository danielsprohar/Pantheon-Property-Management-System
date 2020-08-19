using Microsoft.AspNetCore.Mvc.RazorPages;
using Pantheon.Core.Application.Dto.Reads;
using System.Threading.Tasks;
using Vulcan.Web.Services;

namespace Vulcan.Web.Pages.ParkingSpaces
{
    public class DetailsModel : PageModel
    {
        private readonly IParkingSpaceService _service;

        public ParkingSpaceDto ParkingSpace { get; set; }

        public DetailsModel(IParkingSpaceService service)
        {
            _service = service;
        }

        public async Task OnGetAsync(int id)
        {
            ParkingSpace = await _service.GetParkingSpace(id);
        }
    }
}