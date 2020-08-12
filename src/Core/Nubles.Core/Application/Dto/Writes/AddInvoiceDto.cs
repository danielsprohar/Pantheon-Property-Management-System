using Nubles.Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nubles.Core.Application.Dto.Writes
{
    public class AddInvoiceDto
    {
        [Required]
        public DateTimeOffset DueDate { get; set; }

        [Required]
        public DateTimeOffset BillingPeriodStart { get; set; }

        [Required]
        public DateTimeOffset BillingPeriodEnd { get; set; }

        [StringLength((int)Invoice.DbColumnLength.Comments)]
        public string Comments { get; set; }

        [Required]
        public int? RentalAgreementId { get; set; }

        [Required]
        public int? InvoiceStatusId { get; set; }

        public int? EmployeeId { get; set; }

        [Required]
        public ICollection<AddInvoiceLineDto> InvoiceLines { get; set; } = new HashSet<AddInvoiceLineDto>();
    }
}