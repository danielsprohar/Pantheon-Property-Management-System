using System;
using System.Collections.Generic;

namespace Nubles.Core.Application.Dto.Reads
{
    public class RentalAgreementDto
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public int RecurringDueDate { get; set; }

        public DateTimeOffset? TerminatedOn { get; set; }

        public RentalAgreementTypeDto RentalAgreementType { get; set; }

        public ParkingSpaceDto ParkingSpace { get; set; }

        public ICollection<CustomerDto> Customers { get; set; } = new HashSet<CustomerDto>();
    }
}