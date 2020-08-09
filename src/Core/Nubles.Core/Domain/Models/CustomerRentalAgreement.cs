namespace Nubles.Core.Domain.Models
{
    public class CustomerRentalAgreement
    {
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public int RentalAgreementId { get; set; }
        public RentalAgreement RentalAgreement { get; set; }
    }
}