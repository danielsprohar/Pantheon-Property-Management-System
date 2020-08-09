using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nubles.Core.Domain.Models;

namespace Nubles.Infrastructure.EntityConfigurations
{
    internal class InvoiceConfiguration : AuditableEntityConfiguration<Invoice>
    {
        public override void Configure(EntityTypeBuilder<Invoice> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.BillingPeriodStart).IsRequired();
            builder.Property(e => e.BillingPeriodEnd).IsRequired();
            builder.Property(e => e.DueDate).IsRequired();
            builder.Property(e => e.Comments).HasMaxLength((int)Invoice.DbColumnLength.Comments);

            #region Relationships

            builder.HasOne(e => e.InvoiceStatus)
                .WithMany()
                .HasForeignKey(e => e.InvoiceStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            #endregion Relationships

            #region Owned Types

            builder.OwnsMany(invoice => invoice.InvoiceLines, invoiceLineBuilder =>
            {
                invoiceLineBuilder.Property(e => e.Quantity).IsRequired();

                invoiceLineBuilder.Property(e => e.Description)
                    .HasMaxLength((int)InvoiceLine.DbColumnLength.Description)
                    .IsRequired();

                invoiceLineBuilder.Property(e => e.Price)
                    .HasColumnType(InvoiceLine.DecimalPrecision.Precision)
                    .IsRequired();

                invoiceLineBuilder.Property(e => e.Total)
                    .HasColumnType(InvoiceLine.DecimalPrecision.Precision)
                    .IsRequired();
            });

            #endregion Owned Types
        }
    }
}