using Nubles.Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nubles.Core.Application.Dto.Writes
{
    public class AddInvoiceDto : IValidatableObject
    {
        [Required]
        public DateTime? DueDate { get; set; }

        [Required]
        public DateTime? BillingPeriodStart { get; set; }

        [Required]
        public DateTime? BillingPeriodEnd { get; set; }

        [StringLength((int)Invoice.DbColumnLength.Comments)]
        public string Comments { get; set; }

        [Required]
        public int? RentalAgreementId { get; set; }

        [Required]
        public int? InvoiceStatusId { get; set; }

        public int? EmployeeId { get; set; }

        [Required]
        public ICollection<AddInvoiceLineDto> InvoiceLines { get; set; } = new HashSet<AddInvoiceLineDto>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (BillingPeriodEnd < BillingPeriodStart)
            {
                var message = $"The {nameof(BillingPeriodEnd)} must be greater than or equal to the {BillingPeriodStart} property.";
                yield return new ValidationResult(
                    message,
                    new[] { nameof(BillingPeriodEnd)});
            }
        }
    }
}