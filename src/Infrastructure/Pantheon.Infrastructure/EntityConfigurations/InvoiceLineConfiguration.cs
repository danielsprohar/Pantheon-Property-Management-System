using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pantheon.Core.Domain.Models;

namespace Pantheon.Infrastructure.EntityConfigurations
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

            builder.HasData(GetInvoiceLines());
        }

        private InvoiceLine[] GetInvoiceLines()
        {
            return new[]
            {
                new InvoiceLine
                {
                    InvoiceId = 1,
                    ParkingSpaceId = 1,
                    Quantity = 1,
                    Description = "Space #1; monthly rate of $400.00; electricity and water are included.",
                    Price = 400M,
                    Total = 400M,
                }
            };
        }
    }
}