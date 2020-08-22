using Pantheon.Core.Application.Dto.Reads;
using Pantheon.Core.Application.Dto.Writes;
using Pantheon.Core.Application.Mappings.Resolvers;
using Pantheon.Core.Domain.Models;

namespace Pantheon.Core.Application.Mappings.Profiles
{
    public class ParkingSpaceProfile : EntityProfile
    {
        public ParkingSpaceProfile()
        {
            CreateAddMappings();
            CreateGetMappings();
            CreateUpdateMappings();
        }

        protected override void CreateAddMappings()
        {
            CreateMap<AddParkingSpaceDto, ParkingSpace>()
                .ForMember(e => e.EmployeeId, opts => opts.MapFrom(dto => dto.EmployeeId))
                .ForMember(e => e.ModifiedBy, opts => opts.MapFrom(dto => dto.EmployeeId))
                .ForMember(e => e.ParkingSpaceType, opts => opts.Ignore())
                .ForMember(e => e.CreatedOn, opts => opts.Ignore())
                .ForMember(e => e.ModifiedOn, opts => opts.Ignore())
                .ForMember(e => e.Id, opts => opts.Ignore())
                .ForMember(e => e.RowVersion, opts => opts.Ignore());

            CreateMap<AddParkingSpaceTypeDto, ParkingSpaceType>()
                .ForMember(e => e.Id, opts => opts.Ignore())
                .ForMember(e => e.RowVersion, opts => opts.Ignore());
        }

        protected override void CreateGetMappings()
        {
            CreateMap<ParkingSpace, ParkingSpaceDto>();
            CreateMap<ParkingSpaceType, ParkingSpaceTypeDto>();
        }

        protected override void CreateUpdateMappings()
        {
            CreateMap<UpdateParkingSpaceDto, ParkingSpace>()
                .ForMember(e => e.ParkingSpaceType, opts => opts.Ignore())
                .ForMember(e => e.EmployeeId, opts => opts.Ignore())
                .ForMember(e => e.CreatedOn, opts => opts.Ignore())
                .ForMember(e => e.Id, opts => opts.Ignore())
                .ForMember(e => e.RowVersion, opts => opts.Ignore())
                .ForMember(e => e.ModifiedBy, opts => opts.MapFrom(dto => dto.EmployeeId))
                .ForMember(e => e.ModifiedOn, opts => opts.MapFrom<UtcNowDateResolver>());

            CreateMap<ParkingSpaceDto, UpdateParkingSpaceDto>();
        }
    }
}