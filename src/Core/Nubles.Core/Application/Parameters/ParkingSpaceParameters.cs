namespace Nubles.Core.Application.Parameters
{
    public class ParkingSpaceParameters : QueryParameters
    {
        public bool? IsAvailable { get; set; }

        public int? Amps { get; set; }
    }
}