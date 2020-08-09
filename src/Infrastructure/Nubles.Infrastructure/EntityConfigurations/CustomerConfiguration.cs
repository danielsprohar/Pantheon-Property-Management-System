using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nubles.Core.Domain.Models;
using System.Collections;
using System.Collections.Generic;

namespace Nubles.Infrastructure.EntityConfigurations
{
    internal class CustomerConfiguration : AuditableEntityConfiguration<Customer>
    {
        public override void Configure(EntityTypeBuilder<Customer> builder)
        {
            base.Configure(builder);

            builder.HasIndex(e => new { e.FirstName, e.LastName });

            builder.Property(e => e.FirstName)
                .HasMaxLength((int)Customer.DBColumnLength.Name)
                .IsRequired();

            builder.Property(e => e.MiddleName)
                .HasMaxLength((int)Customer.DBColumnLength.Name);

            builder.Property(e => e.LastName)
                .HasMaxLength((int)Customer.DBColumnLength.Name)
                .IsRequired();

            builder.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsRequired();

            builder.Property(e => e.PhoneNumber)
                .HasMaxLength((int)Customer.DBColumnLength.PhoneNumber)
                .IsRequired();

            // https://tools.ietf.org/html/rfc5321#section-4.5.3
            builder.Property(e => e.Email)
                .HasMaxLength((int)Customer.DBColumnLength.Email);

            #region Owned Types

            builder.OwnsOne(e => e.DriverLicense, dlBuilder =>
            {
                dlBuilder.Property(e => e.Number)
                .HasMaxLength((int)CustomerDriverLicense.DBColumnLength.Number)
                .IsRequired();

                dlBuilder.Property(e => e.State)
                    .HasMaxLength((int)CustomerDriverLicense.DBColumnLength.Number)
                    .IsRequired();
            });


            builder.OwnsMany<CustomerVehicle>(e => e.Vehicles, vehicleBuilder =>
            {
                vehicleBuilder.WithOwner(e => e.Customer)
                    .HasForeignKey(v => v.CustomerId);

                vehicleBuilder.HasKey(e => e.Id);

                vehicleBuilder.Property(e => e.Year).IsRequired();

                vehicleBuilder.Property(e => e.Make)
                    .HasMaxLength((int)CustomerVehicle.DbColumnLength.Make)
                    .IsRequired();

                vehicleBuilder.Property(e => e.Model)
                    .HasMaxLength((int)CustomerVehicle.DbColumnLength.Model)
                    .IsRequired();

                vehicleBuilder.Property(e => e.Color)
                    .HasMaxLength((int)CustomerVehicle.DbColumnLength.Color)
                    .IsRequired();
            });

            #endregion Owned Types

            builder.HasData(GetCustomers());
        }

        private IEnumerable<Customer> GetCustomers()
        {
            return new[]
            {
                new Customer
                {
                    Id = 1,
                    FirstName = "Alice",
                    LastName = "Smite",
                    Gender = 'F',
                    PhoneNumber = "555-555-5555",
                    ModifiedBy = 1,
                    CreatedBy = 1
                }
            };
        }
    }
}