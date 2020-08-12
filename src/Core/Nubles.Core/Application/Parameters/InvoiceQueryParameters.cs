namespace Nubles.Core.Application.Parameters
{
    public class InvoiceQueryParameters : DateQueryParameters
    {
        public int? InvoiceStatusId { get; set; }

        public int? EmployeeId { get; set; }
    }
}