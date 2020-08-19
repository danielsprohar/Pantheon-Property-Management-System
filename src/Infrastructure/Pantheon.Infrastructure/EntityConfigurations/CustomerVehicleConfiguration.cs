using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pantheon.Core.Domain.Models;

namespace Pantheon.Infrastructure.EntityConfigurations
{
    internal class CustomerVehicleConfiguration : EntityConfiguration<CustomerVehicle>
    {
        public override void Configure(EntityTypeBuilder<CustomerVehicle> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Year).IsRequired();

            builder.Property(e => e.Make)
                .HasMaxLength((int)CustomerVehicle.DbColumnLength.Make)
                .IsRequired();

            builder.Property(e => e.Model)
                .HasMaxLength((int)CustomerVehicle.DbColumnLength.Model)
                .IsRequired();

            builder.Property(e => e.Color)
                .HasMaxLength((int)CustomerVehicle.DbColumnLength.Color)
                .IsRequired();

            builder.HasOne(e => e.Customer)
                .WithMany(customer => customer.Vehicles)
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasData(GetCustomerVehicles());
        }

        private CustomerVehicle[] GetCustomerVehicles()
        {
            return new[]
            {
                new CustomerVehicle
                {
                    Id = 1,
                    CustomerId = 1,
                    Year = 2007,
                    Make = "Ford",
                    Model = "Mustang GT",
                    Color = "blue",
                    LicensePlateNumber = "1234-ASD",
                    LicensePlateState = "Texas"
                }
            };
        }
    }
}