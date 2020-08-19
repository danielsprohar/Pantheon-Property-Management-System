using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pantheon.Core.Domain.Models;
using Pantheon.Infrastructure.Constants;
using System;

namespace Pantheon.Infrastructure.EntityConfigurations
{
    internal class InvoiceConfiguration : AuditableEntityConfiguration<Invoice>
    {
        public override void Configure(EntityTypeBuilder<Invoice> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.BillingPeriodStart)
                .HasColumnType(DbConstants.DateDbType)
                .IsRequired();

            builder.Property(e => e.BillingPeriodEnd)
                .HasColumnType(DbConstants.DateDbType)
                .IsRequired();

            builder.Property(e => e.DueDate)
                .HasColumnType(DbConstants.DateDbType)
                .IsRequired();

            builder.Property(e => e.Comments)
                .HasMaxLength((int)Invoice.DbColumnLength.Comments);

            #region Relationships

            builder.HasOne(e => e.InvoiceStatus)
                .WithMany()
                .HasForeignKey(e => e.InvoiceStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            #endregion Relationships

            builder.HasData(GetInvoices());
        }

        private Invoice[] GetInvoices()
        {
            var utcNow = DateTimeOffset.UtcNow;

            return new[]
            {
                new Invoice
                {
                    Id = 1,
                    InvoiceStatusId = 1,
                    RentalAgreementId = 1,
                    EmployeeId = 1,
                    BillingPeriodStart = utcNow.Date,
                    BillingPeriodEnd = utcNow.AddDays(1).Date,
                    DueDate = utcNow.Date
                }
            };
        }
    }
}