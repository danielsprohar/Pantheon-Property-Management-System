using Pantheon.Core.Application.Dto.Reads;
using Pantheon.Core.Application.Dto.Writes;
using Pantheon.Core.Application.Mappings.Resolvers;
using Pantheon.Core.Domain.Models;

namespace Pantheon.Core.Application.Mappings.Profiles
{
    public class RentalAgreementProfile : EntityProfile
    {
        public RentalAgreementProfile()
        {
            CreateAddMappings();
            CreateGetMappings();
            CreateUpdateMappings();
        }

        protected override void CreateAddMappings()
        {
            CreateMap<AddRentalAgreementDto, RentalAgreement>()
                .ForMember(e => e.EmployeeId, opts => opts.MapFrom(dto => dto.EmployeeId))
                .ForMember(e => e.ModifiedBy, opts => opts.MapFrom(dto => dto.EmployeeId))
                .ForMember(e => e.TerminatedOn, opts => opts.Ignore())
                .ForMember(e => e.RentalAgreementType, opts => opts.Ignore())
                .ForMember(e => e.ParkingSpace, opts => opts.Ignore())
                .ForMember(e => e.CustomerRentalAgreements, opts => opts.Ignore())
                .ForMember(e => e.Invoices, opts => opts.Ignore())
                .ForMember(e => e.CreatedOn, opts => opts.Ignore())
                .ForMember(e => e.ModifiedOn, opts => opts.Ignore())
                .ForMember(e => e.Id, opts => opts.Ignore())
                .ForMember(e => e.RowVersion, opts => opts.Ignore());

            CreateMap<AddRentalAgreementTypeDto, RentalAgreementType>()
                .ForMember(e => e.Id, opts => opts.Ignore())
                .ForMember(e => e.RowVersion, opts => opts.Ignore());
        }

        protected override void CreateGetMappings()
        {
            CreateMap<RentalAgreement, RentalAgreementDto>()
                .ForMember(e => e.Customers, opts => opts.Ignore());

            CreateMap<RentalAgreementType, RentalAgreementTypeDto>();
        }

        protected override void CreateUpdateMappings()
        {
            CreateMap<UpdateRentalAgreementDto, RentalAgreement>()
                .ForMember(e => e.ModifiedBy, opts => opts.MapFrom(dto => dto.EmployeeId))
                .ForMember(e => e.Invoices, opts => opts.Ignore())
                .ForMember(e => e.ParkingSpace, opts => opts.Ignore())
                .ForMember(e => e.RentalAgreementType, opts => opts.Ignore())
                .ForMember(e => e.EmployeeId, opts => opts.Ignore())
                .ForMember(e => e.CreatedOn, opts => opts.Ignore())
                .ForMember(e => e.Id, opts => opts.Ignore())
                .ForMember(e => e.RowVersion, opts => opts.Ignore())
                .ForMember(e => e.ModifiedOn, opts => opts.MapFrom<UtcNowDateResolver>());
        }
    }
}