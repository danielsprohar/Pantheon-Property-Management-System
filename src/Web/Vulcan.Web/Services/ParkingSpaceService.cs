﻿using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Nubles.Core.Application.Parameters;
using Nubles.Core.Application.Wrappers.Generics;
using Pantheon.Core.Application.Dto.Reads;
using Pantheon.Core.Application.Dto.Writes;
using Pantheon.Core.Application.Extensions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
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

        public async Task<bool> AddParkingSpace(AddParkingSpaceDto addParkingSpaceDto)
        {
            await SetBearerToken(_httpContextAccessor.HttpContext, _httpClient);

            var employeeId = GetEmployeeId(_httpContextAccessor.HttpContext);

            var routeValues = new RouteValueDictionary(new { employeeId });

            var requestUri = _parkingSpacesUri.AppendRouteValues(routeValues);

            requestUri.AppendRouteValues(routeValues);

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri)
            {
                Content = new StringContent(
                    JsonConvert.SerializeObject(addParkingSpaceDto),
                    System.Text.Encoding.Unicode,
                    MediaTypeNames.Application.Json)
            };

            var responseMessage = await _httpClient.SendAsync(requestMessage);

            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new Exception(responseMessage.ReasonPhrase);
            }

            return true;
        }

        public async Task<ParkingSpaceDto> GetParkingSpace(int id)
        {
            await SetBearerToken(_httpContextAccessor.HttpContext, _httpClient);

            var requestUri = string.Concat(_parkingSpacesUri, "/", id.ToString());

            var responseMessage = await _httpClient.GetAsync(requestUri);

            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new Exception(responseMessage.ReasonPhrase);
            }

            var content = await responseMessage.Content.ReadAsStringAsync();

            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<ParkingSpaceDto>>(content);

            return apiResponse.Data;
        }

        public async Task<IEnumerable<ParkingSpaceDto>> GetParkingSpaces(
            ParkingSpaceQueryParameters parameters = null)
        {
            await SetBearerToken(_httpContextAccessor.HttpContext, _httpClient);

            var requestUri = parameters != null ?
                            _parkingSpacesUri.AppendRouteValues(new RouteValueDictionary(parameters.GetRouteValues())) :
                            _parkingSpacesUri;

            var responseString = await _httpClient.GetStringAsync(requestUri);

            var paginatedApiResponse = JsonConvert
                .DeserializeObject<PaginatedApiResponse<IEnumerable<ParkingSpaceDto>>>(responseString);

            return paginatedApiResponse.Data;
        }

        public async Task<ParkingSpaceTypeDto> GetParkingSpaceType(int id)
        {
            await SetBearerToken(_httpContextAccessor.HttpContext, _httpClient);

            var requestUri = string.Concat(_parkingSpaceTypesUri, "/", id.ToString());

            var responseMessage = await _httpClient.GetAsync(requestUri);

            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new Exception(responseMessage.ReasonPhrase);
            }

            var content = await responseMessage.Content.ReadAsStringAsync();

            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<ParkingSpaceTypeDto>>(content);

            return apiResponse.Data;
        }

        public async Task<IEnumerable<ParkingSpaceTypeDto>> GetParkingSpaceTypes(
            QueryParameters parameters = null)
        {
            await SetBearerToken(_httpContextAccessor.HttpContext, _httpClient);

            var requestUri = parameters != null ?
                            _parkingSpaceTypesUri.AppendRouteValues(new RouteValueDictionary(parameters.GetRouteValues())) :
                            _parkingSpaceTypesUri;

            var responseMessage = await _httpClient.GetAsync(requestUri);

            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new Exception(responseMessage.ReasonPhrase);
            }

            var content = await responseMessage.Content.ReadAsStringAsync();

            var paginatedApiResponse = JsonConvert
                .DeserializeObject<PaginatedApiResponse<IEnumerable<ParkingSpaceTypeDto>>>(content);

            return paginatedApiResponse.Data;
        }

        public async Task<bool> PatchParkingSpace(int id, UpdateParkingSpaceDto dto)
        {
            await SetBearerToken(_httpContextAccessor.HttpContext, _httpClient);

            var employeeId = GetEmployeeId(_httpContextAccessor.HttpContext);

            var routeValues = new RouteValueDictionary(new { employeeId });

            var requestUri = string.Concat(_parkingSpacesUri, "/", id.ToString());

            requestUri.AppendRouteValues(routeValues);

            var patchDoc = new JsonPatchDocument<UpdateParkingSpaceDto>();

            InitializeJsonPatchDocument(patchDoc, dto);

            var requestMessage = new HttpRequestMessage(HttpMethod.Patch, requestUri)
            {
                Content = new StringContent(
                    JsonConvert.SerializeObject(patchDoc),
                    System.Text.Encoding.Unicode,
                    MediaTypeNames.Application.Json)
            };

            var responseMessage = await _httpClient.SendAsync(requestMessage);

            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new Exception(responseMessage.ReasonPhrase);
            }

            return true;
        }

        public async Task<bool> UpdateParkingSpace(int id, UpdateParkingSpaceDto dto)
        {
            await SetBearerToken(_httpContextAccessor.HttpContext, _httpClient);

            var employeeId = GetEmployeeId(_httpContextAccessor.HttpContext);

            var routeValues = new RouteValueDictionary(new { employeeId });

            var requestUri = string.Concat(_parkingSpacesUri, "/", id.ToString());

            requestUri.AppendRouteValues(routeValues);

            var requestMessage = new HttpRequestMessage(HttpMethod.Put, requestUri)
            {
                Content = new StringContent(
                    JsonConvert.SerializeObject(dto),
                    System.Text.Encoding.Unicode,
                    MediaTypeNames.Application.Json)
            };

            var responseMessage = await _httpClient.SendAsync(requestMessage);

            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new Exception(responseMessage.ReasonPhrase);
            }

            return true;
        }

        // =====================================================================================
        // Helper methods
        // =====================================================================================
        private string GetEmployeeId(HttpContext httpContext)
        {
            return httpContext.User.Identity.GetSubjectId();
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