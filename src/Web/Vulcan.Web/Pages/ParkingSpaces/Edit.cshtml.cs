using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Pantheon.Core.Application.Dto.Reads;
using Pantheon.Core.Application.Dto.Writes;
using Pantheon.Core.Application.Services;
using Pantheon.Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vulcan.Web.Extensions;

namespace Vulcan.Web.Pages.ParkingSpaces
{
    public class EditModel : PageModel
    {
        private readonly IParkingSpaceTypeService _parkingSpaceTypeService;
        private readonly IParkingSpaceService _parkingSpaceService;
        private readonly IMapper _mapper;
        private readonly ILogger<EditModel> _logger;

        [BindProperty]
        public ParkingSpaceDto ParkingSpace { get; set; }

        public SelectList ParkingSpaceTypesSelectList { get; set; }

        public SelectList AvailabilitySelectList { get; set; }

        public EditModel(
            IParkingSpaceTypeService parkingSpaceTypeService,
            IParkingSpaceService parkingSpaceService,
            IMapper mapper,
            ILogger<EditModel> logger)
        {
            _parkingSpaceTypeService = parkingSpaceTypeService;
            _parkingSpaceService = parkingSpaceService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task OnGetAsync(int id)
        {
            var apiResponse = await _parkingSpaceService.Get(id);
            ParkingSpace = apiResponse.Data;

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
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var updateDto = _mapper.Map<UpdateParkingSpaceDto>(ParkingSpace);
            updateDto.EmployeeId = new Guid(HttpContext.GetUserId());

            var apiResponse = await _parkingSpaceService.Update(ParkingSpace.Id, updateDto);

            if (!apiResponse.Succeeded)
            {
                // TODO: handle unsuccessful request
                _logger.LogError(apiResponse.Message);

                foreach (var error in apiResponse.Errors)
                {
                    _logger.LogError(error);
                }

                return this.HandleUnsuccessfulApiRequest(apiResponse);
            }

            return RedirectToPage("./Details", new { Id = ParkingSpace.Id });
        }
    }
}