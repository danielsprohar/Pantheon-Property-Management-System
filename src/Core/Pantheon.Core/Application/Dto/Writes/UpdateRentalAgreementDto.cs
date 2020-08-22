using System;
using System.ComponentModel.DataAnnotations;

namespace Pantheon.Core.Application.Dto.Writes
{
    public class UpdateRentalAgreementDto
    {
        public int? RecurringDueDate { get; set; }

        public DateTimeOffset? TerminatedOn { get; set; }

        public int? RentalAgreementTypeId { get; set; }

        public int? ParkingSpaceId { get; set; }

        [Required]
        public Guid EmployeeId { get; set; }
    }
}