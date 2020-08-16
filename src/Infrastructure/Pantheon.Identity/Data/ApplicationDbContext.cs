using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pantheon.Identity.Models;
using System;

namespace Pantheon.Identity.Data
{
    public class ApplicationDbContext
        : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>(user =>
            {
                user.Property(e => e.DateCreated)
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("GETUTCDATE()")
                    .IsRequired();
            });
        }
    }
}