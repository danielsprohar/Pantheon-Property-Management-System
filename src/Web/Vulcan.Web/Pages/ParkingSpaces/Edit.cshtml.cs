using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Pantheon.Core.Application.Dto.Reads;
using Pantheon.Core.Application.Dto.Writes;
using Pantheon.Core.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vulcan.Web.Services;

namespace Vulcan.Web.Pages.ParkingSpaces
{
    public class EditModel : PageModel
    {
        private readonly IParkingSpaceService _service;
        private readonly IMapper _mapper;

        [BindProperty]
        public ParkingSpaceDto ParkingSpace { get; set; }

        public SelectList ParkingSpaceTypesSelectList { get; set; }

        public SelectList AvailabilitySelectList { get; set; }

        public EditModel(IParkingSpaceService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task OnGetAsync(int id)
        {
            var apiResponse = await _service.GetParkingSpaceAsync(id);
            ParkingSpace = apiResponse.Data;

            var paginatedApiResponse = await _service.GetParkingSpaceTypesAsync();
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
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var updateDto = _mapper.Map<UpdateParkingSpaceDto>(ParkingSpace);

            var apiResponse = await _service.UpdateParkingSpaceAsync(ParkingSpace.Id, updateDto);

            if (!apiResponse.Succeeded)
            {
                // TODO: show error reason
                return Page();
            }

            return RedirectToPage("./Details", new { Id = ParkingSpace.Id });
        }
    }
}