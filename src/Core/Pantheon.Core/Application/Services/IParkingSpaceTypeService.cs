using System.Collections.Generic;
using System.Threading.Tasks;
using Pantheon.Core.Application.Dto.Reads;
using Pantheon.Core.Application.Dto.Writes;
using Pantheon.Core.Application.Parameters;
using Pantheon.Core.Application.Wrappers;
using Pantheon.Core.Application.Wrappers.Generics;

namespace Pantheon.Core.Application.Services
{
    public interface IParkingSpaceTypeService
    {
        Task<ApiResponse<ParkingSpaceTypeDto>> Add(AddParkingSpaceTypeDto dto);

        Task<ApiResponse> Delete(int id);

        Task<ApiResponse<ParkingSpaceTypeDto>> Get(int id);

        Task<PaginatedApiResponse<IEnumerable<ParkingSpaceTypeDto>>> GetPaginatedList(QueryParameters parameters = null);
    }
}