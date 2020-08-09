namespace Nubles.Core.Application.Parameters
{
    public class ParkingSpaceParameters : QueryParameters
    {
        /// <summary>
        /// Returns all Units based on availability.
        /// </summary>
        public bool? IsAvailable { get; set; }

        /// <summary>
        /// Returns all Units based on the given amp capacity.
        /// </summary>
        public int? AmpCapacity { get; set; }

        public ParkingSpaceParameters() : base()
        {
        }

        public ParkingSpaceParameters(int pageNumber, int pageSize, bool? isAvailable, int? ampCapacity)
            : base(pageNumber, pageSize)
        {
            IsAvailable = isAvailable;
            AmpCapacity = ampCapacity;
        }
    }
}