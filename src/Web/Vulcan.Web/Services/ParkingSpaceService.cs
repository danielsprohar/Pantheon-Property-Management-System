using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Nubles.Core.Application.Dto.Reads;
using Nubles.Core.Application.Dto.Writes;
using Nubles.Core.Application.Parameters;
using Nubles.Core.Application.Wrappers.Generics;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Vulcan.Web.Services
{
    public class ParkingSpaceService : IParkingSpaceService
    {
        private readonly ILogger<ParkingSpaceService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient;

        public ParkingSpaceService(
            ILogger<ParkingSpaceService> logger,
            IHttpContextAccessor httpContextAccessor,
            HttpClient httpClient)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _httpClient = httpClient;
        }

        public Task<bool> AddParkingSpace(AddParkingSpaceDto parkingSpaceDto)
        {
            throw new NotImplementedException();
        }

        public Task<ParkingSpaceDto> GetParkingSpace(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ParkingSpaceDto>> GetParkingSpaces(
            ParkingSpaceQueryParameters parameters)
        {
            var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var requestUri = "https://localhost:5001/api/v1/parking-spaces";

            // TODO: append the queryParameters to the requestUri

            if (string.IsNullOrWhiteSpace(requestUri))
            {
                throw new Exception("Could not retrieve the ParkingSpaces resource url from appsettings.json");
            }

            var responseString = await _httpClient.GetStringAsync(requestUri);

            var paginatedApiResponse = JsonConvert.DeserializeObject<PaginatedApiResponse<IEnumerable<ParkingSpaceDto>>>(responseString);

            var parkingSpaces = paginatedApiResponse.Data;

            return parkingSpaces;
        }
    }
}