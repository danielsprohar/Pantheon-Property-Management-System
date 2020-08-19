﻿using Microsoft.AspNetCore.Identity;
using System;

namespace Pantheon.Identity.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public DateTimeOffset DateCreated { get; set; }
    }
}