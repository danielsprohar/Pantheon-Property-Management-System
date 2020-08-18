using Nubles.Core.Application.Dto.Reads;
using Nubles.Core.Application.Dto.Writes;
using Nubles.Core.Application.Parameters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Vulcan.Web.Services
{
    public interface IParkingSpaceService
    {
        Task<bool> AddParkingSpace(AddParkingSpaceDto parkingSpaceDto);

        Task<IEnumerable<ParkingSpaceDto>> GetParkingSpaces(ParkingSpaceQueryParameters parameters);

        Task<ParkingSpaceDto> GetParkingSpace(int id);
    }
}