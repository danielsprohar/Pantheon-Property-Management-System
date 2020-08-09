using Nubles.Core.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Nubles.Core.Application.Dto.Writes
{
    public class AddRentalAgreementDto
    {
        // TODO: create validation attribute

        /// <summary>
        /// This should be an integer value in the range [1,31],
        /// that represents the day of the month that the
        /// account owner's recurring rate is due.
        /// </summary>
        [Required]
        public int RecurringDueDate { get; set; }

        [StringLength((int)RentalAgreement.DBColumnLength.Comments)]
        public string Comments { get; set; }

        [Required]
        public int RentalAgreementTypeId { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public AddCustomerDto Customer { get; set; }
    }
}