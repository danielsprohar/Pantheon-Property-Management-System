using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nubles.Core.Domain.Models;
using Nubles.Infrastructure.Constants;
using System.Data;

namespace Nubles.Infrastructure.EntityConfigurations
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
        }
    }
}