using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nubles.Core.Domain.Models;
using System.Collections.Generic;

namespace Nubles.Infrastructure.EntityConfigurations
{
    internal class RentalAgreementConfiguration : AuditableEntityConfiguration<RentalAgreement>
    {
        public override void Configure(EntityTypeBuilder<RentalAgreement> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.RecurringDueDate).IsRequired();

            #region Relationships

            builder.HasOne(e => e.ParkingSpace)
                .WithMany()
                .HasForeignKey(e => e.ParkingSpaceId);

            builder.HasOne(e => e.RentalAgreementType)
                .WithMany()
                .HasForeignKey(e => e.RentalAgreementTypeId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            builder.HasMany(e => e.Invoices)
                .WithOne(i => i.RentalAgreement)
                .HasForeignKey(i => i.RentalAgreementId)
                .OnDelete(DeleteBehavior.Restrict);

            #endregion Relationships

            builder.HasData(GetRentalAgreements());
        }

        private IEnumerable<RentalAgreement> GetRentalAgreements()
        {
            return new[]
            {
                new RentalAgreement
                {
                    Id = 1,
                    RecurringDueDate = 1,
                    CreatedBy = 1,
                    ModifiedBy = 1
                }
            };
        }
    }
}