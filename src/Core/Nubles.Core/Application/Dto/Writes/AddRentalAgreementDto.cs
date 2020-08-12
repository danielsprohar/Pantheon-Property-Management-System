using Nubles.Core.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Nubles.Core.Application.Dto.Writes
{
    public class AddRentalAgreementDto
    {
        /// <summary>
        /// This should be an integer value in the range [1,31],
        /// that represents the day of the month when rent is due.
        /// </summary>
        [Required]
        [Range(1, 31)]
        public int? RecurringDueDate { get; set; }

        [StringLength((int)RentalAgreement.DbColumnLength.Comments)]
        public string Comments { get; set; }

        [Required]
        public int? RentalAgreementTypeId { get; set; }

        public int? EmployeeId { get; set; }

        [Required]
        public int? ParkingSpaceId { get; set; }

        [Required]
        public AddCustomerDto Customer { get; set; }
    }
}