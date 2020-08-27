namespace Pantheon.Core.Application.Parameters
{
    public class ParkingSpaceQueryParameters : QueryParameters
    {
        public bool? IsAvailable { get; set; }

        public int? Amps { get; set; }

        public int? ParkingSpaceTypeId { get; set; }

        public ParkingSpaceQueryParameters()
        {

        }

        public ParkingSpaceQueryParameters(ParkingSpaceQueryParameters other)
            : base(other)
        {
            Amps = other.Amps;
            IsAvailable = other.IsAvailable;
            ParkingSpaceTypeId = other.ParkingSpaceTypeId;
        }
    }
}