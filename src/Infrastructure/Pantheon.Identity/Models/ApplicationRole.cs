using Microsoft.AspNetCore.Identity;
using System;

namespace Pantheon.Identity.Models
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public class Roles
        {
            public const string Admin = "administrator";
            public const string User = "user";
        }
    }
}