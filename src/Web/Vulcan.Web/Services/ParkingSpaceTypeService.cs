using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Pantheon.Core.Application.Dto.Reads;
using Pantheon.Core.Application.Dto.Writes;
using Pantheon.Core.Application.Extensions;
using Pantheon.Core.Application.Parameters;
using Pantheon.Core.Application.Services;
using Pantheon.Core.Application.Wrappers;
using Pantheon.Core.Application.Wrappers.Generics;
using Vulcan.Web.Extensions;
using Vulcan.Web.Options;

namespace Vulcan.Web.Services
{
    public class ParkingSpaceTypeService : IParkingSpaceTypeService
    {
        private readonly string _requestUri;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient;
        private readonly ILogger<ParkingSpaceTypeService> _logger;
        private readonly HermesApiOptions _options;

        public ParkingSpaceTypeService(
            IHttpContextAccessor httpContextAccessor,
            HttpClient httpClient,
            ILogger<ParkingSpaceTypeService> logger,
            IOptions<HermesApiOptions> options)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClient = httpClient;
            _logger = logger;
            _options = options.Value;

            _requestUri = string.Concat(_options.BaseAddress,
                                              _options.Version.V1,
                                              _options.ResourcePath.ParkingSpaces);
        }


        public Task<ApiResponse<ParkingSpaceTypeDto>> Add(AddParkingSpaceTypeDto dto)
        {
            throw new System.NotImplementedException();
        }

        public Task<ApiResponse> Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ApiResponse<ParkingSpaceTypeDto>> Get(int id)
        {
            await _httpContextAccessor.HttpContext.SetBearerToken(_httpClient);

            var uri = string.Concat(_requestUri, "/", id.ToString());

            var response = await _httpClient.PingWebApi(HttpMethod.Get, uri);

            if (!response.IsSuccessStatusCode)
            {
                return new ApiResponse<ParkingSpaceTypeDto>
                {
                    Message = response.ReasonPhrase,
                    StatusCode = response.StatusCode
                };
            }

            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<ApiResponse<ParkingSpaceTypeDto>>(content);
        }

        public async Task<PaginatedApiResponse<IEnumerable<ParkingSpaceTypeDto>>> GetPaginatedList(
            QueryParameters parameters = null)
        {
            await _httpContextAccessor.HttpContext.SetBearerToken(_httpClient);

            var uri = parameters != null ?
                            _requestUri.AppendRouteValues(new RouteValueDictionary(parameters.GetRouteValues())) :
                            _requestUri;

            var response = await _httpClient.PingWebApi(HttpMethod.Get, uri);

            if (!response.IsSuccessStatusCode)
            {
                return new PaginatedApiResponse<IEnumerable<ParkingSpaceTypeDto>>(response.ReasonPhrase)
                {
                    StatusCode = response.StatusCode
                };
            }

            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert
                .DeserializeObject<PaginatedApiResponse<IEnumerable<ParkingSpaceTypeDto>>>(content);
        }
    }
}