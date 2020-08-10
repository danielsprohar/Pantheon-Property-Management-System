namespace Nubles.Core.Application.Parameters
{
    public class ParkingSpaceParameters : QueryParameters
    {
        public ParkingSpaceParameters() : base()
        {
        }

        public bool? IsAvailable { get; set; }

        public int? Amps { get; set; }
    }
}