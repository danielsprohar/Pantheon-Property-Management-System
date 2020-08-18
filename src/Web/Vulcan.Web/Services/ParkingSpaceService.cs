using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Nubles.Core.Application.Dto.Reads;
using Nubles.Core.Application.Dto.Writes;
using Nubles.Core.Application.Extensions;
using Nubles.Core.Application.Parameters;
using Nubles.Core.Application.Wrappers.Generics;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Vulcan.Web.Options;

namespace Vulcan.Web.Services
{
    public class ParkingSpaceService : IParkingSpaceService
    {
        private readonly ILogger<ParkingSpaceService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient;
        private readonly HermesApiOptions _hermesApiOptions;
        private readonly LinkGenerator _linkGenerator;

        public ParkingSpaceService(
            ILogger<ParkingSpaceService> logger,
            IHttpContextAccessor httpContextAccessor,
            HttpClient httpClient,
            IOptions<HermesApiOptions> options,
            LinkGenerator linkGenerator)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _httpClient = httpClient;
            _hermesApiOptions = options.Value;
            _linkGenerator = linkGenerator;
        }

        public Task<bool> AddParkingSpace(AddParkingSpaceDto parkingSpaceDto)
        {
            // var userId = _httpContextAccessor.HttpContext.User.Identity.GetSubjectId();
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

            var routeValues = new RouteValueDictionary(parameters.GetRouteValues());

            var requestUri = string.Concat(_hermesApiOptions.BaseAddress,
                                           _hermesApiOptions.Version.V1,
                                           _hermesApiOptions.ResourcePath.ParkingSpaces)
                                   .AppendRouteValues(routeValues);

            _logger.LogInformation($"URI: {requestUri}");

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