using Pantheon.Core.Application.Dto.Reads;
using Pantheon.Core.Application.Dto.Writes;
using Pantheon.Core.Application.Parameters;
using Pantheon.Core.Application.Wrappers;
using Pantheon.Core.Application.Wrappers.Generics;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Vulcan.Web.Services
{
    public interface IParkingSpaceService
    {
        Task<ApiResponse<ParkingSpaceDto>> AddParkingSpaceAsync(AddParkingSpaceDto dto);

        Task<ApiResponse> DeleteParkingSpaceAsync(int id);

        Task<PaginatedApiResponse<IEnumerable<ParkingSpaceDto>>> GetParkingSpacesAsync(ParkingSpaceQueryParameters parameters = null);

        Task<ApiResponse<ParkingSpaceDto>> GetParkingSpaceAsync(int id);

        Task<ApiResponse<ParkingSpaceTypeDto>> GetParkingSpaceTypeAsync(int id);

        Task<PaginatedApiResponse<IEnumerable<ParkingSpaceTypeDto>>> GetParkingSpaceTypesAsync(QueryParameters parameters = null);

        /// <summary>
        /// Updates an existing ParkingSpace via HttpPatch
        /// </summary>
        /// <param name="id">RouteValue: The Id of an existing ParkingSpace</param>
        /// <param name="dto">RequestBody: The ParkingSpace attributes to be updated</param>
        /// <param name="employeeId">Query Parameter: the Id of the Employee that is modifying </param>
        /// <returns></returns>
        Task<ApiResponse> PatchParkingSpaceAsync(int id, UpdateParkingSpaceDto dto);

        /// <summary>
        /// Updates an existig ParkingSpace via HttpPut
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<ApiResponse> UpdateParkingSpaceAsync(int id, UpdateParkingSpaceDto dto);
    }
}