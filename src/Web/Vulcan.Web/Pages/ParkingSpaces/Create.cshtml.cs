using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Pantheon.Core.Application.Dto.Writes;
using Pantheon.Core.Application.Services;
using Pantheon.Core.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vulcan.Web.Extensions;

namespace Vulcan.Web.Pages.ParkingSpaces
{
    public class CreateModel : PageModel
    {
        private readonly IParkingSpaceTypeService _parkingSpaceTypeService;
        private readonly IParkingSpaceService _parkingSpaceService;
        private readonly ILogger<CreateModel> _logger;

        [BindProperty]
        public AddParkingSpaceDto ParkingSpace { get; set; }

        public SelectList ParkingSpaceTypesSelectList { get; set; }

        public SelectList AvailabilitySelectList { get; set; }

        public CreateModel(
            IParkingSpaceTypeService parkingSpaceTypeService,
            IParkingSpaceService parkingSpaceService,
            ILogger<CreateModel> logger)
        {
            _parkingSpaceTypeService = parkingSpaceTypeService;
            _parkingSpaceService = parkingSpaceService;
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            var paginatedApiResponse = await _parkingSpaceTypeService.GetPaginatedList();
            var parkingSpaceTypes = paginatedApiResponse.Data;

            ParkingSpaceTypesSelectList = new SelectList(
                items: parkingSpaceTypes,
                dataValueField: nameof(ParkingSpaceType.Id),
                dataTextField: nameof(ParkingSpaceType.SpaceType));

            var dictionary = new Dictionary<string, bool>
            {
                { "false", false },
                { "true", true }
            };

            AvailabilitySelectList = new SelectList(dictionary, "Key", "Value");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var apiResponse = await _parkingSpaceService.Add(ParkingSpace);

            if (!apiResponse.Succeeded)
            {
                _logger.LogError(apiResponse.Message);
                foreach (var error in apiResponse.Errors)
                {
                    _logger.LogError(error);
                }

                return this.HandleUnsuccessfulApiRequest(apiResponse);
            }

            var parkingSpaceDto = apiResponse.Data;

            return RedirectToPage("./Details", new { Id = parkingSpaceDto.Id });
        }
    }
}