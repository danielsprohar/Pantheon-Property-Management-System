namespace Pantheon.Core.Application.Parameters
{
    public class PaymentQueryParameters : DateQueryParameters
    {
        public bool? IsRefund { get; set; }

        public int? CustomerId { get; set; }

        public int? PaymentMethodId { get; set; }
    }
}