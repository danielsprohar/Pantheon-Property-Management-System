using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pantheon.Identity.Constants;
using Pantheon.Identity.Models;
using System;

namespace Pantheon.Identity.EntityConfigurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(e => e.DateCreated)
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("GETUTCDATE()")
                    .IsRequired();

            builder.HasData(GetApplicationUsers());
        }

        private ApplicationUser[] GetApplicationUsers()
        {
            var passwordHasher = new PasswordHasher<ApplicationUser>();
            var userName = "Alice";
            var email = "alice.smith@example.com";

            var user = new ApplicationUser
            {
                Id = new Guid(DbDefaultValues.UserId),
                UserName = userName,
                NormalizedUserName = userName.ToUpperInvariant(),
                Email = email,
                NormalizedEmail = email.ToUpperInvariant(),
                EmailConfirmed = true,
                PhoneNumber = "(555) 555-5555",
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            // TODO: fix this; the hash produced here does not result in the same hash in the application
            user.PasswordHash = passwordHasher.HashPassword(user, "Password123$");

            return new ApplicationUser[] { user };
        }
    }
}