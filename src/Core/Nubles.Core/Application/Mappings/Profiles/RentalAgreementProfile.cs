using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Nubles.Core.Application.Dto.Reads;
using Nubles.Core.Application.Dto.Writes;
using Nubles.Core.Domain.Models;

namespace Nubles.Core.Application.Mappings.Profiles
{
    public class RentalAgreementProfile : EntityProfile
    {
        public RentalAgreementProfile()
        {
            CreateAddMappings();
            CreateGetMappings();
            CreateUpdateMappings();
        }

        public override void CreateAddMappings()
        {
            CreateMap<AddRentalAgreementDto, RentalAgreement>()
                .ForMember(e => e.TerminatedOn, opts => opts.Ignore())
                .ForMember(e => e.RentalAgreementType, opts => opts.Ignore())
                .ForMember(e => e.ParkingSpace, opts => opts.Ignore())
                .ForMember(e => e.CustomerRentalAgreements, opts => opts.Ignore())
                .ForMember(e => e.Invoices, opts => opts.Ignore())
                .ForMember(e => e.CreatedBy, opts => opts.Ignore())
                .ForMember(e => e.CreatedOn, opts => opts.Ignore())
                .ForMember(e => e.ModifiedBy, opts => opts.Ignore())
                .ForMember(e => e.ModifiedOn, opts => opts.Ignore())
                .ForMember(e => e.Id, opts => opts.Ignore())
                .ForMember(e => e.RowVersion, opts => opts.Ignore());

            CreateMap<AddRentalAgreementTypeDto, RentalAgreementType>()
                .ForMember(e => e.Id, opts => opts.Ignore())
                .ForMember(e => e.RowVersion, opts => opts.Ignore());
        }

        public override void CreateGetMappings()
        {
            CreateMap<RentalAgreement, RentalAgreementDto>()
                .ForMember(e => e.Customers, opts => opts.Ignore());

            CreateMap<RentalAgreementType, RentalAgreementTypeDto>();
        }

        public override void CreateUpdateMappings()
        {
            CreateMap<UpdateRentalAgreementDto, RentalAgreement>()
                .ForMember(e => e.Invoices, opts => opts.Ignore())
                .ForMember(e => e.ParkingSpace, opts => opts.Ignore())
                .ForMember(e => e.RentalAgreementType, opts => opts.Ignore())
                .ForMember(e => e.CreatedBy, opts => opts.Ignore())
                .ForMember(e => e.CreatedOn, opts => opts.Ignore())
                .ForMember(e => e.ModifiedBy, opts => opts.Ignore())
                .ForMember(e => e.ModifiedOn, opts => opts.Ignore())
                .ForMember(e => e.Id, opts => opts.Ignore())
                .ForMember(e => e.RowVersion, opts => opts.Ignore());

            CreateMap<JsonPatchDocument<UpdateRentalAgreementDto>, JsonPatchDocument<RentalAgreement>>();
            CreateMap<Operation<UpdateRentalAgreementDto>, Operation<RentalAgreement>>();
        }
    }
}