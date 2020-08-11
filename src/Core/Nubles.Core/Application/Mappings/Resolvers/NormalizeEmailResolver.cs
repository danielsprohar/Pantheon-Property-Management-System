using AutoMapper;
using Nubles.Core.Application.Dto.Writes;
using Nubles.Core.Domain.Models;

namespace Nubles.Core.Application.Mappings.Resolvers
{
    internal class NormalizeEmailResolver : IValueResolver<AddCustomerDto, Customer, string>
    {
        public string Resolve(
            AddCustomerDto source,
            Customer destination,
            string destMember,
            ResolutionContext context)
        {
            return source.Email.ToUpper();
        }
    }
}