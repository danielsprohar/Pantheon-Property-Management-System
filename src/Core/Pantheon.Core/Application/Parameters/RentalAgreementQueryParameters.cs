namespace Pantheon.Core.Application.Parameters
{
    public class RentalAgreementQueryParameters : DateQueryParameters
    {
        public int? EmployeeId { get; set; }

        public bool? IsActive { get; set; }

        public int? ParkingSpaceId { get; set; }
    }
}