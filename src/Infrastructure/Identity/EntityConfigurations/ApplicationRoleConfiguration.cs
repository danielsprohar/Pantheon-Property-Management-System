using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pantheon.Core.Domain.Constants;
using Pantheon.Identity.Models;
using System;

namespace Pantheon.Identity.EntityConfigurations
{
    public class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            builder.HasData(GetApplicationRoles());
        }

        private ApplicationRole[] GetApplicationRoles()
        {
            return new ApplicationRole[]
            {
                new ApplicationRole
                {
                    Id = new Guid(DbDefaultValues.AdminRoleId),
                    Name = DbDefaultValues.AdminRole,
                    NormalizedName = DbDefaultValues.AdminRole.ToUpperInvariant()
                },
                new ApplicationRole
                {
                    Id = new Guid(DbDefaultValues.UserRoleId),
                    Name = DbDefaultValues.UserRole,
                    NormalizedName = DbDefaultValues.UserRole.ToUpperInvariant()
                }
            };
        }
    }
}