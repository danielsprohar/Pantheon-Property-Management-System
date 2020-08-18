using System;

namespace Pantheon.Core.Application.Dto.Reads
{
    public class PaymentDto
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }

        public DateTimeOffset Date { get; set; }

        public bool? IsRefund { get; set; }

        public int PaymentMethodId { get; set; }

        public int CustomerId { get; set; }
    }
}