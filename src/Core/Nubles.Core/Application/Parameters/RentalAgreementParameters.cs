namespace Nubles.Core.Application.Parameters
{
    public class RentalAgreementParameters : QueryParameters
    {
        public RentalAgreementParameters() : base()
        {
        }

        public int? CustomerId { get; set; }

        public int? Employee { get; set; }

        public bool? IsActive { get; set; }

        public int? ParkingSpaceId { get; set; }
    }
}