using AutoMapper;

namespace Pantheon.Core.Application.Mappings
{
    public abstract class EntityProfile : Profile
    {
        protected abstract void CreateAddMappings();

        protected abstract void CreateGetMappings();

        protected abstract void CreateUpdateMappings();
    }
}