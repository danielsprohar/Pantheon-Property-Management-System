using System;
using System.ComponentModel.DataAnnotations;

namespace Pantheon.Core.Application.Dto.Writes
{
    public class AddPaymentDto
    {
        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTimeOffset Date { get; set; }

        public bool? IsRefund { get; set; }

        [Required]
        public int? PaymentMethodId { get; set; }

        [Required]
        public int? CustomerId { get; set; }

        [Required]
        public int? InvoiceId { get; set; }
    }
}