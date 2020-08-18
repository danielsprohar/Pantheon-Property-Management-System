using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace Pantheon.Core.Application.Mappings
{
    public class NublesProfile : Profile
    {
        public NublesProfile()
        {
            CreateMap(typeof(JsonPatchDocument<>), typeof(JsonPatchDocument<>));
            CreateMap(typeof(Operation<>), typeof(Operation<>));
        }
    }
}