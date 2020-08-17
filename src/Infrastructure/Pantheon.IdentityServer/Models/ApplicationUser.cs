using Microsoft.AspNetCore.Identity;
using System;

namespace Pantheon.IdentityServer.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public DateTimeOffset DateCreated { get; set; }
    }
}