using Nubles.Core.Domain.Base;
using System;
using System.Collections.Generic;

namespace Nubles.Core.Domain.Models
{
    public class Payment : AuditableEntity
    {
        public static class DbColumnType
        {
            public const string Decimal = "DECIMAL(10, 2)";
        }

        public decimal Amount { get; set; }

        public DateTimeOffset Date { get; set; }

        public bool? IsRefund { get; set; }

        #region Navigation Properties

        public int PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public ICollection<InvoicePayment> InvoicePayments { get; set; } = new List<InvoicePayment>();

        #endregion Navigation Properties
    }
}