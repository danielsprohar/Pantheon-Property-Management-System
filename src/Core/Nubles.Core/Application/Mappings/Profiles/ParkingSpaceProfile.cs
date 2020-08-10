﻿using Nubles.Core.Application.Dto.Reads;
using Nubles.Core.Application.Dto.Writes;
using Nubles.Core.Domain.Models;

namespace Nubles.Core.Application.Mappings.Profiles
{
    public class ParkingSpaceProfile : EntityProfile
    {
        public ParkingSpaceProfile()
        {
            CreateAddMappings();
            CreateGetMappings();
            CreateUpdateMappings();
        }

        public override void CreateAddMappings()
        {
            CreateMap<AddParkingSpaceDto, ParkingSpace>()
                .ForMember(e => e.ParkingSpaceType, opts => opts.Ignore())
                .ForMember(e => e.CreatedBy, opts => opts.Ignore())
                .ForMember(e => e.CreatedOn, opts => opts.Ignore())
                .ForMember(e => e.ModifiedBy, opts => opts.Ignore())
                .ForMember(e => e.ModifiedOn, opts => opts.Ignore())
                .ForMember(e => e.Id, opts => opts.Ignore())
                .ForMember(e => e.RowVersion, opts => opts.Ignore());

            CreateMap<AddParkingSpaceTypeDto, ParkingSpaceType>()
                .ForMember(e => e.Id, opts => opts.Ignore())
                .ForMember(e => e.RowVersion, opts => opts.Ignore());
        }

        public override void CreateGetMappings()
        {
            CreateMap<ParkingSpace, ParkingSpaceDto>();
            CreateMap<ParkingSpaceType, ParkingSpaceTypeDto>();
        }

        public override void CreateUpdateMappings()
        {
            CreateMap<UpdateParkingSpaceDto, ParkingSpace>()
                .ForMember(e => e.ParkingSpaceType, opts => opts.Ignore())
                .ForMember(e => e.CreatedBy, opts => opts.Ignore())
                .ForMember(e => e.CreatedOn, opts => opts.Ignore())
                .ForMember(e => e.ModifiedBy, opts => opts.Ignore())
                .ForMember(e => e.ModifiedOn, opts => opts.Ignore())
                .ForMember(e => e.Id, opts => opts.Ignore())
                .ForMember(e => e.RowVersion, opts => opts.Ignore());
        }
    }
}