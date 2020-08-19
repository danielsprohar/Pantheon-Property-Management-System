using Nubles.Core.Application.Parameters;
using Pantheon.Core.Application.Dto.Reads;
using Pantheon.Core.Application.Dto.Writes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Vulcan.Web.Services
{
    public interface IParkingSpaceService
    {
        Task<bool> AddParkingSpace(AddParkingSpaceDto parkingSpaceDto);

        Task<IEnumerable<ParkingSpaceDto>> GetParkingSpaces(ParkingSpaceQueryParameters parameters = null);

        Task<ParkingSpaceDto> GetParkingSpace(int id);

        Task<ParkingSpaceTypeDto> GetParkingSpaceType(int id);

        Task<IEnumerable<ParkingSpaceTypeDto>> GetParkingSpaceTypes(QueryParameters parameters = null);

        /// <summary>
        /// Updates an existing ParkingSpace via HttpPatch
        /// </summary>
        /// <param name="id">RouteValue: The Id of an existing ParkingSpace</param>
        /// <param name="dto">RequestBody: The ParkingSpace attributes to be updated</param>
        /// <param name="employeeId">Query Parameter: the Id of the Employee that is modifying </param>
        /// <returns></returns>
        Task<bool> PatchParkingSpace(int id, UpdateParkingSpaceDto dto);

        /// <summary>
        /// Updates an existig ParkingSpace via HttpPut
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<bool> UpdateParkingSpace(int id, UpdateParkingSpaceDto dto);
    }
}