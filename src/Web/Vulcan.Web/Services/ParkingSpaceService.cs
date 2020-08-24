using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Pantheon.Core.Application.Dto.Reads;
using Pantheon.Core.Application.Dto.Writes;
using Pantheon.Core.Application.Extensions;
using Pantheon.Core.Application.Parameters;
using Pantheon.Core.Application.Wrappers;
using Pantheon.Core.Application.Wrappers.Generics;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Security.Claims;
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

        private readonly string _parkingSpacesUri;
        private readonly string _parkingSpaceTypesUri;

        public ParkingSpaceService(
            ILogger<ParkingSpaceService> logger,
            IHttpContextAccessor httpContextAccessor,
            HttpClient httpClient,
            IOptions<HermesApiOptions> options)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _httpClient = httpClient;
            _hermesApiOptions = options.Value;

            _parkingSpacesUri = string.Concat(_hermesApiOptions.BaseAddress,
                                              _hermesApiOptions.Version.V1,
                                              _hermesApiOptions.ResourcePath.ParkingSpaces);

            _parkingSpaceTypesUri = string.Concat(_hermesApiOptions.BaseAddress,
                                                  _hermesApiOptions.Version.V1,
                                                  _hermesApiOptions.ResourcePath.ParkingSpaceTypes);
        }

        public async Task<ApiResponse<ParkingSpaceDto>> AddParkingSpaceAsync(AddParkingSpaceDto dto)
        {
            await SetBearerToken(_httpContextAccessor.HttpContext, _httpClient);

            dto.EmployeeId = new Guid(GetEmployeeId(_httpContextAccessor.HttpContext));

            var responseMessage = await InvokeWebRequest(HttpMethod.Post, _parkingSpacesUri, dto);

            if (!responseMessage.IsSuccessStatusCode)
            {
                return new ApiResponse<ParkingSpaceDto>
                {
                    Message = responseMessage.ReasonPhrase,
                    StatusCode = responseMessage.StatusCode
                };
            }

            var responseBody = await responseMessage.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<ApiResponse<ParkingSpaceDto>>(responseBody);
        }

        public Task<ApiResponse> DeleteParkingSpaceAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<ParkingSpaceDto>> GetParkingSpaceAsync(int id)
        {
            await SetBearerToken(_httpContextAccessor.HttpContext, _httpClient);

            var requestUri = string.Concat(_parkingSpacesUri, "/", id.ToString());

            var responseMessage = await InvokeWebRequest(HttpMethod.Get, requestUri);

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

        public async Task<PaginatedApiResponse<IEnumerable<ParkingSpaceDto>>> GetParkingSpacesAsync(
            ParkingSpaceQueryParameters parameters = null)
        {
            await SetBearerToken(_httpContextAccessor.HttpContext, _httpClient);

            var requestUri = parameters != null ?
                            _parkingSpacesUri.AppendRouteValues(new RouteValueDictionary(parameters.GetRouteValues())) :
                            _parkingSpacesUri;

            var responseMessage = await InvokeWebRequest(HttpMethod.Get, requestUri);

            if (!responseMessage.IsSuccessStatusCode)
            {
                return new PaginatedApiResponse<IEnumerable<ParkingSpaceDto>>(responseMessage.ReasonPhrase)
                {
                    StatusCode = responseMessage.StatusCode
                };
            }

            var content = await responseMessage.Content.ReadAsStringAsync();

            return JsonConvert
                .DeserializeObject<PaginatedApiResponse<IEnumerable<ParkingSpaceDto>>>(content);
        }

        public async Task<ApiResponse<ParkingSpaceTypeDto>> GetParkingSpaceTypeAsync(int id)
        {
            await SetBearerToken(_httpContextAccessor.HttpContext, _httpClient);

            var requestUri = string.Concat(_parkingSpaceTypesUri, "/", id.ToString());

            var responseMessage = await InvokeWebRequest(HttpMethod.Get, requestUri);

            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new Exception(responseMessage.ReasonPhrase);
            }

            var content = await responseMessage.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<ApiResponse<ParkingSpaceTypeDto>>(content);
        }

        public async Task<PaginatedApiResponse<IEnumerable<ParkingSpaceTypeDto>>> GetParkingSpaceTypesAsync(
            QueryParameters parameters = null)
        {
            await SetBearerToken(_httpContextAccessor.HttpContext, _httpClient);

            var requestUri = parameters != null ?
                            _parkingSpaceTypesUri.AppendRouteValues(new RouteValueDictionary(parameters.GetRouteValues())) :
                            _parkingSpaceTypesUri;

            var responseMessage = await InvokeWebRequest(HttpMethod.Get, requestUri);

            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new Exception(responseMessage.ReasonPhrase);
            }

            var content = await responseMessage.Content.ReadAsStringAsync();

            return JsonConvert
                .DeserializeObject<PaginatedApiResponse<IEnumerable<ParkingSpaceTypeDto>>>(content);
        }

        public async Task<ApiResponse> PatchParkingSpaceAsync(int id, UpdateParkingSpaceDto dto)
        {
            await SetBearerToken(_httpContextAccessor.HttpContext, _httpClient);

            var employeeId = GetEmployeeId(_httpContextAccessor.HttpContext);

            var routeValues = new RouteValueDictionary(new { employeeId });

            var requestUri = string.Concat(_parkingSpacesUri, "/", id.ToString());

            requestUri.AppendRouteValues(routeValues);

            var patchDoc = new JsonPatchDocument<UpdateParkingSpaceDto>();

            InitializeJsonPatchDocument(patchDoc, dto);

            var responseMessage = await InvokeWebRequest(HttpMethod.Patch, requestUri, dto);

            if (!responseMessage.IsSuccessStatusCode)
            {
                return new ApiResponse($"StatusCode: {responseMessage.StatusCode}; Reason: {responseMessage.ReasonPhrase}");
            }

            return new ApiResponse("Parking Space was patched", true);
        }

        public async Task<ApiResponse> UpdateParkingSpaceAsync(int id, UpdateParkingSpaceDto dto)
        {
            await SetBearerToken(_httpContextAccessor.HttpContext, _httpClient);

            var employeeId = GetEmployeeId(_httpContextAccessor.HttpContext);

            var routeValues = new RouteValueDictionary(new { employeeId });

            var requestUri = string.Concat(_parkingSpacesUri, "/", id.ToString());

            requestUri.AppendRouteValues(routeValues);

            var responseMessage = await InvokeWebRequest(HttpMethod.Put, requestUri, dto);

            if (!responseMessage.IsSuccessStatusCode)
            {
                return new ApiResponse($"StatusCode: {responseMessage.StatusCode}; Reason: {responseMessage.ReasonPhrase}");
            }

            return new ApiResponse("Parking Space was updated", true);
        }

        // =====================================================================================
        // Helper methods
        // =====================================================================================
        private async Task<HttpResponseMessage> InvokeWebRequest(HttpMethod method, string uri, object content = null)
        {
            var request = new HttpRequestMessage(method, uri);

            if (content != null)
            {
                request.Content = new StringContent(
                    JsonConvert.SerializeObject(content),
                    System.Text.Encoding.Unicode,
                    MediaTypeNames.Application.Json);
            }

            return await _httpClient.SendAsync(request);
        }

        private string GetEmployeeId(HttpContext httpContext)
        {
            //return httpContext.User.Identity.GetSubjectId();
            return httpContext.User.FindFirstValue("sub");
        }

        private async Task SetBearerToken(HttpContext httpContext, HttpClient httpClient)
        {
            var accessToken = await httpContext.GetTokenAsync("access_token");

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        private void InitializeJsonPatchDocument(
            JsonPatchDocument<UpdateParkingSpaceDto> patchDoc,
            UpdateParkingSpaceDto dto)
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