using AutoMapper;
using Pantheon.Core.Domain.Base;
using System;

namespace Pantheon.Core.Application.Mappings.Resolvers
{
    /// <summary>
    /// Initializes the Destination entity property with the a UTC timestamp.
    /// </summary>
    internal class UtcNowDateResolver : IValueResolver<object, AuditableEntity, DateTimeOffset>
    {
        public DateTimeOffset Resolve(
            object source,
            AuditableEntity destination,
            DateTimeOffset destMember,
            ResolutionContext context)
        {
            return DateTimeOffset.UtcNow;
        }
    }
}