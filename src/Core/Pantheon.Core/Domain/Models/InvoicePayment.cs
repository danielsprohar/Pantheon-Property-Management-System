namespace Pantheon.Core.Domain.Models
{
    public class InvoicePayment
    {
        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; }

        public int PaymentId { get; set; }
        public Payment Payment { get; set; }
    }
}