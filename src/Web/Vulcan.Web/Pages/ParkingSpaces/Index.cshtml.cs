using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Pantheon.Core.Application.Dto.Reads;
using Pantheon.Core.Application.Parameters;
using Pantheon.Core.Application.Services;
using Pantheon.Core.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Vulcan.Web.Pages.ParkingSpaces
{
    public class IndexModel : PageModel
    {
        private readonly IParkingSpaceService _parkingSpaceService;
        private readonly IParkingSpaceTypeService _parkingSpaceTypeService;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(
            IParkingSpaceService service,
            IParkingSpaceTypeService parkingSpaceTypeService,
            ILogger<IndexModel> logger)
        {
            _parkingSpaceService = service;
            _parkingSpaceTypeService = parkingSpaceTypeService;
            _logger = logger;
        }

        [BindProperty(SupportsGet = true)]
        public ParkingSpaceQueryParameters QueryParameters { get; set; }

        public SelectList AmpsSelectList { get; set; }
        public SelectList AvailabilitySelectList { get; set; }
        public SelectList ParkingSpaceTypesSelectList { get; set; }
        public IEnumerable<ParkingSpaceDto> ParkingSpaces { get; set; }

        public async Task OnGetAsync()
        {
            await InitializeSelectLists();

            ParkingSpaces = await FetchParkingSpaces(QueryParameters);

            var url = Url.Page("./Index", QueryParameters);
            ViewData["URL"] = url;
        }

        // ==========================================================================
        // Helper methods
        // ==========================================================================

        public bool IsFiltered()
        {
            var q = QueryParameters;
            return q.Amps.HasValue || q.IsAvailable.HasValue || q.ParkingSpaceTypeId.HasValue;
        }

        private async Task InitializeSelectLists()
        {
            AmpsSelectList = GetAmpsSelectList();
            AvailabilitySelectList = GetAvailablitiySelectList();
            ParkingSpaceTypesSelectList = await GetParkingSpaceTypeSelectList();
        }

        private SelectList GetAmpsSelectList()
        {
            var dictionary = new Dictionary<string, int>
            {
                { "30 amps", 30 },
                { "50 amps", 50 }
            };

            return new SelectList(dictionary, "Value", "Key");
        }

        private SelectList GetAvailablitiySelectList()
        {
            var dictionary = new Dictionary<string, bool>
            {
                { "Not Available", false },
                { "Available", true }
            };

            return new SelectList(dictionary, "Value", "Key");
        }

        private async Task<SelectList> GetParkingSpaceTypeSelectList()
        {
            var paginatedApiResponse = await _parkingSpaceTypeService.GetPaginatedList();
            var parkingSpaceTypes = paginatedApiResponse.Data;

            return new SelectList(
                items: parkingSpaceTypes,
                dataValueField: nameof(ParkingSpaceType.Id),
                dataTextField: nameof(ParkingSpaceType.SpaceType));
        }

        private async Task<IEnumerable<ParkingSpaceDto>> FetchParkingSpaces(
            ParkingSpaceQueryParameters parameters)
        {
            var response = await _parkingSpaceService.GetPaginatedList(parameters);

            if (!response.Succeeded)
            {
                // TODO: handle unsuccessful response
                _logger.LogError(response.Message);

                foreach (var error in response.Errors)
                {
                    _logger.LogError(error);
                }
            }

            return response.Data;
        }
    }
}