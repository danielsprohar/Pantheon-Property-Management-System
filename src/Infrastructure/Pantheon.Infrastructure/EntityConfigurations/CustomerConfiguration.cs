﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pantheon.Core.Domain.Constants;
using Pantheon.Core.Domain.Models;
using System;
using System.Collections.Generic;

namespace Pantheon.Infrastructure.EntityConfigurations
{
    internal class CustomerConfiguration : AuditableEntityConfiguration<Customer>
    {
        public override void Configure(EntityTypeBuilder<Customer> builder)
        {
            base.Configure(builder);

            builder.HasIndex(e => new { e.FirstName, e.LastName });

            builder.Property(e => e.FirstName)
                .HasMaxLength((int)Customer.DbColumnLength.Name)
                .IsRequired();

            builder.Property(e => e.MiddleName)
                .HasMaxLength((int)Customer.DbColumnLength.Name);

            builder.Property(e => e.LastName)
                .HasMaxLength((int)Customer.DbColumnLength.Name)
                .IsRequired();

            builder.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsRequired();

            builder.Property(e => e.PhoneNumber)
                .HasMaxLength((int)Customer.DbColumnLength.PhoneNumber)
                .IsRequired();

            // https://tools.ietf.org/html/rfc5321#section-4.5.3
            builder.Property(e => e.Email)
                .HasMaxLength((int)Customer.DbColumnLength.Email);

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
                    ModifiedBy = new Guid(DbDefaultValues.UserId),
                    EmployeeId = new Guid(DbDefaultValues.UserId)
                }
            };
        }
    }
}