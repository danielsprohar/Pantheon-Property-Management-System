using System;

namespace Pantheon.Core.Application.Parameters
{
    public class RentalAgreementQueryParameters : DateQueryParameters
    {
        public Guid? EmployeeId { get; set; }

        public bool? IsActive { get; set; }

        public int? ParkingSpaceId { get; set; }
    }
}