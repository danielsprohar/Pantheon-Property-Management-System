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

        Task<IEnumerable<ParkingSpaceDto>> GetParkingSpaces(ParkingSpaceQueryParameters parameters);

        Task<ParkingSpaceDto> GetParkingSpace(int id);
    }
}