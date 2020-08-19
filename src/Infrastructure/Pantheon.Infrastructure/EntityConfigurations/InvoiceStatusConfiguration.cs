using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pantheon.Core.Domain.Models;

namespace Pantheon.Infrastructure.EntityConfigurations
{
    internal class InvoiceStatusConfiguration : EntityConfiguration<InvoiceStatus>
    {
        public override void Configure(EntityTypeBuilder<InvoiceStatus> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Description)
                .HasMaxLength((int)InvoiceStatus.DbColumnLength.Description)
                .IsRequired();

            builder.HasData(GetInvoiceStatuses());
        }

        private InvoiceStatus[] GetInvoiceStatuses()
        {
            return new[]
            {
                new InvoiceStatus
                {
                    Id = 1,
                    Description = InvoiceStatus.Status.AwaitingPayment
                },
                new InvoiceStatus
                {
                    Id = 2,
                    Description = InvoiceStatus.Status.BadDebt
                },
                new InvoiceStatus
                {
                    Id = 3,
                    Description = InvoiceStatus.Status.Draft
                },
                new InvoiceStatus
                {
                    Id = 4,
                    Description = InvoiceStatus.Status.Paid
                },
                new InvoiceStatus
                {
                    Id = 5,
                    Description = InvoiceStatus.Status.PastDue
                },
                new InvoiceStatus
                {
                    Id = 6,
                    Description = InvoiceStatus.Status.Partial
                },
                new InvoiceStatus
                {
                    Id = 7,
                    Description = InvoiceStatus.Status.Void
                }
            };
        }
    }
}