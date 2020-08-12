namespace Nubles.Core.Application.Parameters
{
    public class RentalAgreementParameters : QueryParameters
    {
        public int? EmployeeId { get; set; }

        public bool? IsActive { get; set; }

        public int? ParkingSpaceId { get; set; }
    }
}