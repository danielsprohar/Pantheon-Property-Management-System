namespace Pantheon.Core.Application.Parameters
{
    public class ParkingSpaceQueryParameters : QueryParameters
    {
        public bool? IsAvailable { get; set; }

        public int? Amps { get; set; }

        public int? ParkingSpaceTypeId { get; set; }
    }
}