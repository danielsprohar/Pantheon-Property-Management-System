using AutoMapper;
using Pantheon.Core.Application.Dto.Writes;
using Pantheon.Core.Domain.Models;

namespace Pantheon.Core.Application.Mappings.Resolvers
{
    internal class NormalizeEmailResolver : IValueResolver<AddCustomerDto, Customer, string>
    {
        public string Resolve(
            AddCustomerDto source,
            Customer destination,
            string destMember,
            ResolutionContext context)
        {
            return source.Email.ToUpperInvariant();
        }
    }
}