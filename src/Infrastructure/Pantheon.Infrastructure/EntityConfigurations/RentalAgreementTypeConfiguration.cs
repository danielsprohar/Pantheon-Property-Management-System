using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pantheon.Core.Domain.Models;

namespace Pantheon.Infrastructure.EntityConfigurations
{
    internal class RentalAgreementTypeConfiguration : EntityConfiguration<RentalAgreementType>
    {
        public override void Configure(EntityTypeBuilder<RentalAgreementType> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.AgreementType)
                .HasMaxLength((int)RentalAgreementType.DbColumnLength.AgreementType)
                .IsRequired();

            builder.HasData(GetRentalAgreementTypes());
        }

        private RentalAgreementType[] GetRentalAgreementTypes()
        {
            return new RentalAgreementType[]
            {
                new RentalAgreementType
                {
                    Id = 1,
                    AgreementType = RentalAgreementType.Type.Daily
                },
                new RentalAgreementType
                {
                    Id = 2,
                    AgreementType = RentalAgreementType.Type.Monthly
                },
                new RentalAgreementType
                {
                    Id = 3,
                    AgreementType = RentalAgreementType.Type.Weekly
                }
            };
        }
    }
}