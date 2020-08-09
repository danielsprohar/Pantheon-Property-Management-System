using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nubles.Core.Domain.Base;
using Nubles.Infrastructure.Constants;

namespace Nubles.Infrastructure.EntityConfigurations
{
    internal class AuditableEntityConfiguration<T> : EntityConfiguration<T> where T : AuditableEntity
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.CreatedOn)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql(DbConstants.UtcDate);

            builder.Property(e => e.ModifiedOn)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql(DbConstants.UtcDate);

            //builder.HasOne<ApplicationUser>()
            //    .WithMany()
            //    .OnDelete(DeleteBehavior.Restrict)
            //    .HasForeignKey(e => e.CreatedBy);
        }
    }
}