using AutoMapper;

namespace Nubles.Core.Application.Mappings
{
    public abstract class EntityProfile : Profile
    {
        public abstract void CreateAddMappings();

        public abstract void CreateGetMappings();

        public abstract void CreateUpdateMappings();
    }
}