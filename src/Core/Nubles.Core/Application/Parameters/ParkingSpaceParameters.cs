namespace Nubles.Core.Application.Parameters
{
    public class ParkingSpaceParameters : QueryParameters
    {
        public bool? IsAvailable { get; set; }

        public int? AmpCapacity { get; set; }
    }
}