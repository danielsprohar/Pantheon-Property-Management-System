using System.Collections.Generic;
using System.Threading.Tasks;
using Pantheon.Core.Application.Dto.Reads;
using Pantheon.Core.Application.Dto.Writes;
using Pantheon.Core.Application.Parameters;
using Pantheon.Core.Application.Wrappers;
using Pantheon.Core.Application.Wrappers.Generics;

namespace Pantheon.Core.Application.Services
{
    public interface IParkingSpaceService
    {
        Task<ApiResponse<ParkingSpaceDto>> Add(AddParkingSpaceDto dto);

        Task<ApiResponse> Delete(int id);

        Task<ApiResponse<ParkingSpaceDto>> Get(int id);

        Task<PaginatedApiResponse<IEnumerable<ParkingSpaceDto>>> GetPaginatedList(ParkingSpaceQueryParameters parameters);

        Task<ApiResponse> Patch(int id, PatchParkingSpaceDto dto);

        Task<ApiResponse> Update(int id, UpdateParkingSpaceDto dto);
    }
}