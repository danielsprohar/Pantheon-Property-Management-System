using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nubles.Core.Domain.Models;

namespace Nubles.Infrastructure.EntityConfigurations
{
    internal class InvoiceLineConfiguration : IEntityTypeConfiguration<InvoiceLine>
    {
        public void Configure(EntityTypeBuilder<InvoiceLine> builder)
        {
            builder.HasKey(e => new { e.InvoiceId, e.ParkingSpaceId });

            builder.HasOne(e => e.Invoice)
                .WithMany(invoice => invoice.InvoiceLines)
                .HasForeignKey(e => e.InvoiceId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.ParkingSpace)
                .WithMany()
                .HasForeignKey(e => e.ParkingSpaceId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(e => e.Quantity).IsRequired();

            builder.Property(e => e.Description)
                .HasMaxLength((int)InvoiceLine.DbColumnLength.Description)
                .IsRequired();

            builder.Property(e => e.Price)
                .HasColumnType(InvoiceLine.DecimalPrecision.Precision)
                .IsRequired();

            builder.Property(e => e.Total)
                .HasColumnType(InvoiceLine.DecimalPrecision.Precision)
                .IsRequired();
        }
    }
}