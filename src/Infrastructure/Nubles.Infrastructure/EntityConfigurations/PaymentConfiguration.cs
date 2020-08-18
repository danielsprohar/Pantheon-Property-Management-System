using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pantheon.Core.Domain.Models;

namespace Nubles.Infrastructure.EntityConfigurations
{
    internal class PaymentConfiguration : AuditableEntityConfiguration<Payment>
    {
        public override void Configure(EntityTypeBuilder<Payment> builder)
        {
            base.Configure(builder);

            builder.HasKey(e => e.Id);

            builder.Property(e => e.IsRefund)
                .HasDefaultValue(false)
                .IsRequired();

            builder.Property(e => e.Amount)
                .HasColumnType(Payment.DbColumnType.Decimal)
                .IsRequired();

            #region Relationships

            builder.HasOne(e => e.Customer)
                .WithMany(c => c.Payments)
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.PaymentMethod)
                .WithMany()
                .HasForeignKey(e => e.PaymentMethodId)
                .OnDelete(DeleteBehavior.Restrict);

            #endregion Relationships
        }
    }
}