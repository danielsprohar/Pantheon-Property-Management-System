using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pantheon.Core.Domain.Models;

namespace Pantheon.Infrastructure.EntityConfigurations
{
    internal class ParkingSpaceTypeConfiguration : EntityConfiguration<ParkingSpaceType>
    {
        public override void Configure(EntityTypeBuilder<ParkingSpaceType> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.SpaceType)
                .HasMaxLength((int)ParkingSpaceType.DbColumnLength.SpaceType)
                .IsRequired();

            builder.HasData(GetParkingSpaceTypes());
        }

        private ParkingSpaceType[] GetParkingSpaceTypes()
        {
            return new[]
            {
                new ParkingSpaceType
                {
                    Id = 1,
                    SpaceType = ParkingSpaceType.Type.RV
                },
                new ParkingSpaceType
                {
                    Id = 2,
                    SpaceType = ParkingSpaceType.Type.MobileHome
                },
                new ParkingSpaceType
                {
                    Id = 3,
                    SpaceType = ParkingSpaceType.Type.House
                }
            };
        }
    }
}