using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pantheon.Core.Application.Dto.Reads;
using Pantheon.Core.Application.Dto.Writes;
using Pantheon.Core.Application.Extensions;
using Pantheon.Core.Application.Parameters;
using Pantheon.Core.Application.Services;
using Pantheon.Core.Application.Wrappers;
using Pantheon.Core.Application.Wrappers.Generics;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Vulcan.Web.Extensions;
using Vulcan.Web.Options;

namespace Vulcan.Web.Services
{
    public class ParkingSpaceService : IParkingSpaceService
    {
        private readonly ILogger<ParkingSpaceService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient;
        private readonly HermesApiOptions _options;

        private readonly string _requestUri;

        public ParkingSpaceService(
            ILogger<ParkingSpaceService> logger,
            IHttpContextAccessor httpContextAccessor,
            HttpClient httpClient,
            IOptions<HermesApiOptions> options)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _httpClient = httpClient;
            _options = options.Value;

            _requestUri = string.Concat(_options.BaseAddress,
                                              _options.Version.V1,
                                              "/",
                                              _options.Resources.ParkingSpaces);
        }

        public async Task<ApiResponse<ParkingSpaceDto>> Add(AddParkingSpaceDto dto)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            await httpContext.SetBearerToken(_httpClient); ;

            dto.EmployeeId = new Guid(httpContext.GetUserId());

            var response = await _httpClient.SendAsync(HttpMethod.Post, _requestUri, dto);

            if (!response.IsSuccessStatusCode)
            {
                return new ApiResponse<ParkingSpaceDto>
                {
                    Message = response.ReasonPhrase,
                    StatusCode = response.StatusCode
                };
            }

            var responseBody = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<ApiResponse<ParkingSpaceDto>>(responseBody);
        }

        public Task<ApiResponse> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<ParkingSpaceDto>> Get(int id)
        {
            await _httpContextAccessor.HttpContext.SetBearerToken(_httpClient);

            var requestUri = string.Concat(_requestUri, "/", id.ToString());

            var responseMessage = await _httpClient.SendAsync(HttpMethod.Get, requestUri);

            if (!responseMessage.IsSuccessStatusCode)
            {
                return new ApiResponse<ParkingSpaceDto>
                {
                    Message = responseMessage.ReasonPhrase,
                    StatusCode = responseMessage.StatusCode
                };
            }

            var content = await responseMessage.Content.ReadAsStringAsync();

            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<ParkingSpaceDto>>(content);

            return apiResponse;
        }

        public async Task<PaginatedApiResponse<IEnumerable<ParkingSpaceDto>>> GetPaginatedList(
            ParkingSpaceQueryParameters parameters = null)
        {
            await _httpContextAccessor.HttpContext.SetBearerToken(_httpClient);

            var requestUri = parameters != null ?
                            _requestUri.AppendRouteValues(new RouteValueDictionary(parameters.GetRouteValues())) :
                            _requestUri;

            var response = await _httpClient.SendAsync(HttpMethod.Get, requestUri);

            if (!response.IsSuccessStatusCode)
            {
                return new PaginatedApiResponse<IEnumerable<ParkingSpaceDto>>(response.ReasonPhrase)
                {
                    StatusCode = response.StatusCode
                };
            }

            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert
                .DeserializeObject<PaginatedApiResponse<IEnumerable<ParkingSpaceDto>>>(content);
        }

        public async Task<ApiResponse> Patch(int id, PatchParkingSpaceDto dto)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            await httpContext.SetBearerToken(_httpClient);

            var routeValues = new RouteValueDictionary(new { employeeId = httpContext.GetUserId() });

            var requestUri = string.Concat(_requestUri, "/", id.ToString());

            requestUri.AppendRouteValues(routeValues);

            var patchDoc = new JsonPatchDocument<PatchParkingSpaceDto>();

            InitializeJsonPatchDocument(patchDoc, dto);

            var responseMessage = await _httpClient.SendAsync(HttpMethod.Patch, requestUri, dto);

            if (!responseMessage.IsSuccessStatusCode)
            {
                return new ApiResponse
                {
                    Message = responseMessage.ReasonPhrase,
                    StatusCode = responseMessage.StatusCode
                };
            }

            return new ApiResponse($"ParkingSpace.Id {id} was patched", true);
        }

        public async Task<ApiResponse> Update(int id, UpdateParkingSpaceDto dto)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            await httpContext.SetBearerToken(_httpClient);

            var routeValues = new RouteValueDictionary(new { employeeId = httpContext.GetUserId() });

            var requestUri = string.Concat(_requestUri, "/", id.ToString());

            requestUri.AppendRouteValues(routeValues);

            var responseMessage = await _httpClient.SendAsync(HttpMethod.Put, requestUri, dto);

            if (!responseMessage.IsSuccessStatusCode)
            {
                return new ApiResponse
                {
                    Message = responseMessage.ReasonPhrase,
                    StatusCode = responseMessage.StatusCode
                };
            }

            return new ApiResponse($"ParkingSpace.Id {id} was updated", true);
        }

        // =====================================================================================
        // Helper methods
        // =====================================================================================
        private void InitializeJsonPatchDocument(
            JsonPatchDocument<PatchParkingSpaceDto> patchDoc,
            PatchParkingSpaceDto dto)
        {
            if (dto.Amps.HasValue)
            {
                patchDoc.Replace(e => e.Amps, dto.Amps.Value);
            }
            if (!string.IsNullOrWhiteSpace(dto.Comments))
            {
                patchDoc.Replace(e => e.Comments, dto.Comments);
            }
            if (!string.IsNullOrWhiteSpace(dto.Description))
            {
                patchDoc.Replace(e => e.Description, dto.Description);
            }
            if (dto.IsAvailable.HasValue)
            {
                patchDoc.Replace(e => e.IsAvailable, dto.IsAvailable.Value);
            }
            if (!string.IsNullOrWhiteSpace(dto.Name))
            {
                patchDoc.Replace(e => e.Name, dto.Name);
            }
            if (dto.ParkingSpaceTypeId.HasValue)
            {
                patchDoc.Replace(e => e.ParkingSpaceTypeId, dto.ParkingSpaceTypeId.Value);
            }
            if (dto.RecurringRate.HasValue)
            {
                patchDoc.Replace(e => e.RecurringRate, dto.RecurringRate.Value);
            }
        }
    }
}