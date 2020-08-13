using System;
using System.Collections.Generic;

namespace Nubles.Core.Application.Dto.Reads
{
    public class InvoiceDto
    {
        public int Id { get; set; }

        public string DueDate { get; set; }

        public string BillingPeriodStart { get; set; }

        public string BillingPeriodEnd { get; set; }

        public string Comments { get; set; }

        // TODO: add EmployeeDto

        public RentalAgreementDto RentalAgreement { get; set; }

        public InvoiceStatusDto InvoiceStatus { get; set; }

        public ICollection<InvoiceLineDto> InvoiceLines { get; set; } = new List<InvoiceLineDto>();
    }
}
