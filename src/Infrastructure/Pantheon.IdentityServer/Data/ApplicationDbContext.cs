using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pantheon.IdentityServer.Models;
using System;

namespace Pantheon.IdentityServer.Data
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