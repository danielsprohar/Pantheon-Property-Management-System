using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pantheon.Core.Domain.Models;

namespace Pantheon.Infrastructure.EntityConfigurations
{
    internal class CustomerRentalAgreementConfiguration : IEntityTypeConfiguration<CustomerRentalAgreement>
    {
        public void Configure(EntityTypeBuilder<CustomerRentalAgreement> builder)
        {
            builder.HasKey(e => new { e.CustomerId, e.RentalAgreementId });

            builder.HasOne(e => e.Customer)
                .WithMany(c => c.CustomerRentalAgreements)
                .HasForeignKey(e => e.CustomerId);

            builder.HasOne(e => e.RentalAgreement)
                .WithMany(c => c.CustomerRentalAgreements)
                .HasForeignKey(e => e.RentalAgreementId);

            builder.HasData(GetCustomerRentalAgreements());
        }

        private CustomerRentalAgreement[] GetCustomerRentalAgreements()
        {
            return new[]
            {
                new CustomerRentalAgreement
                {
                    CustomerId = 1,
                    RentalAgreementId = 1
                }
            };
        }
    }
}