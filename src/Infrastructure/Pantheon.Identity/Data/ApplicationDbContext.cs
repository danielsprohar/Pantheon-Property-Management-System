﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pantheon.Identity.Constants;
using Pantheon.Identity.EntityConfigurations;
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

            builder.ApplyConfiguration(new ApplicationUserConfiguration());
            builder.ApplyConfiguration(new ApplicationRoleConfiguration());

            builder.Entity<IdentityUserRole<Guid>>(builder =>
            {
                builder.HasData(new[]
                {
                    new IdentityUserRole<Guid>
                    { 
                        UserId = new Guid(DbDefaultValues.UserId),
                        RoleId = new Guid(DbDefaultValues.AdminRoleId)
                    },
                    new IdentityUserRole<Guid>
                    {
                        UserId = new Guid(DbDefaultValues.UserId),
                        RoleId = new Guid(DbDefaultValues.UserRoleId)
                    }
                });
            });
        }
    }
}