﻿using Pantheon.Core.Domain.Base;
using System;
using System.Collections.Generic;

namespace Pantheon.Core.Domain.Models
{
    public class Invoice : AuditableEntity
    {
        public enum DbColumnLength
        {
            Comments = 2048
        }

        public DateTime DueDate { get; set; }
        public DateTime BillingPeriodStart { get; set; }
        public DateTime BillingPeriodEnd { get; set; }
        public string Comments { get; set; }

        #region Navigation Properties

        public int RentalAgreementId { get; set; }
        public RentalAgreement RentalAgreement { get; set; }

        public int InvoiceStatusId { get; set; }
        public InvoiceStatus InvoiceStatus { get; set; }

        public ICollection<InvoiceLine> InvoiceLines { get; set; } = new List<InvoiceLine>();
        public ICollection<InvoicePayment> InvoicePayments { get; set; } = new List<InvoicePayment>();

        #endregion Navigation Properties
    }
}