using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pantheon.Core.Domain.Models;

namespace Nubles.Infrastructure.EntityConfigurations
{
    internal class InvoicePaymentConfiguration : IEntityTypeConfiguration<InvoicePayment>
    {
        public void Configure(EntityTypeBuilder<InvoicePayment> builder)
        {
            builder.HasKey(e => new { e.InvoiceId, e.PaymentId });

            builder.HasOne(e => e.Invoice)
                .WithMany(a => a.InvoicePayments)
                .HasForeignKey(e => e.InvoiceId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Payment)
                .WithMany(t => t.InvoicePayments)
                .HasForeignKey(e => e.PaymentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}