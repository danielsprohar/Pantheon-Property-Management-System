using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nubles.Core.Domain.Models;
using System.Collections.Generic;

namespace Nubles.Infrastructure.EntityConfigurations
{
    internal class ParkingSpaceConfiguration : AuditableEntityConfiguration<ParkingSpace>
    {
        public override void Configure(EntityTypeBuilder<ParkingSpace> builder)
        {
            base.Configure(builder);

            builder.HasIndex(e => e.Name).IsUnique();

            builder.Property(e => e.Name)
                .HasMaxLength((int)ParkingSpace.DbColumnLength.Name)
                .IsRequired();

            builder.Property(e => e.Description)
                .HasMaxLength((int)ParkingSpace.DbColumnLength.Description);

            builder.Property(e => e.Comments)
                .HasMaxLength((int)ParkingSpace.DbColumnLength.Commments);

            builder.Property(e => e.RecurringRate)
                .HasColumnType(ParkingSpace.DbColumnType.Decimal)
                .IsRequired();

            builder.Property(e => e.IsAvailable)
                .HasDefaultValue(true)
                .IsRequired();

            #region Relationships

            builder.HasOne(e => e.ParkingSpaceType)
                .WithMany()
                .HasForeignKey(e => e.ParkingSpaceTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            #endregion Relationships

            builder.HasData(GetParkingSpaces());
        }

        private IEnumerable<ParkingSpace> GetParkingSpaces()
        {
            var units = new List<ParkingSpace>();

            for (var i = 1; i <= 32; i++)
            {
                units.Add(new ParkingSpace
                {
                    Id = i,
                    Name = i.ToString(),
                    Amps = (int?)ParkingSpace.AmpCapacity.Thirty,
                    RecurringRate = 400,
                    ParkingSpaceTypeId = 1,
                    CreatedBy = 1,
                    ModifiedBy = 1
                });
            }

            return units;
        }
    }
}