using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Nubles.Core.Application.Dto.Reads;
using Nubles.Core.Application.Dto.Writes;
using Nubles.Core.Application.Mappings.Resolvers;
using Nubles.Core.Domain.Models;

namespace Nubles.Core.Application.Mappings
{
    public class NublesProfile : Profile
    {
        public NublesProfile()
        {
            CreateCustomerMappings();
            CreateParkingSpaceMappings();
            CreateRentalAgreementMappings();
        }

        private void CreateCustomerMappings()
        {
            CreateMap<Customer, CustomerDto>();
            CreateMap<AddCustomerDto, Customer>()
                .ForMember(e => e.IsActive, opts => opts.Ignore())
                .ForMember(e => e.Vehicles, opts => opts.Ignore())
                .ForMember(e => e.CustomerRentalAgreements, opts => opts.Ignore())
                .ForMember(e => e.Payments, opts => opts.Ignore())
                .ForMember(e => e.CreatedBy, opts => opts.Ignore())
                .ForMember(e => e.CreatedOn, opts => opts.Ignore())
                .ForMember(e => e.ModifiedBy, opts => opts.Ignore())
                .ForMember(e => e.ModifiedOn, opts => opts.Ignore())
                .ForMember(e => e.Id, opts => opts.Ignore())
                .ForMember(e => e.RowVersion, opts => opts.Ignore())
                .ForMember(e => e.NormalizedEmail, opts => opts.MapFrom<NormalizeEmailResolver>());

            CreateMap<CustomerVehicle, CustomerVehicleDto>();
            CreateMap<AddCustomerVehicleDto, CustomerVehicle>()
                .ForMember(e => e.Id, opts => opts.Ignore())
                .ForMember(e => e.RowVersion, opts => opts.Ignore())
                .ForMember(e => e.Customer, opts => opts.Ignore());

            CreateMap<CustomerDriverLicense, CustomerDriverLicenseDto>();
            CreateMap<AddCustomerDriverLicenseDto, CustomerDriverLicense>();
        }

        private void CreateParkingSpaceMappings()
        {
            CreateMap<ParkingSpace, ParkingSpaceDto>();
            CreateMap<AddParkingSpaceDto, ParkingSpace>()
                .ForMember(e => e.ParkingSpaceType, opts => opts.Ignore())
                .ForMember(e => e.CreatedBy, opts => opts.Ignore())
                .ForMember(e => e.CreatedOn, opts => opts.Ignore())
                .ForMember(e => e.ModifiedBy, opts => opts.Ignore())
                .ForMember(e => e.ModifiedOn, opts => opts.Ignore())
                .ForMember(e => e.Id, opts => opts.Ignore())
                .ForMember(e => e.RowVersion, opts => opts.Ignore());

            CreateMap<UpdateParkingSpaceDto, ParkingSpace>()
                .ForMember(e => e.ParkingSpaceType, opts => opts.Ignore())
                .ForMember(e => e.CreatedBy, opts => opts.Ignore())
                .ForMember(e => e.CreatedOn, opts => opts.Ignore())
                .ForMember(e => e.ModifiedBy, opts => opts.Ignore())
                .ForMember(e => e.ModifiedOn, opts => opts.Ignore())
                .ForMember(e => e.Id, opts => opts.Ignore())
                .ForMember(e => e.RowVersion, opts => opts.Ignore());

            CreateMap<JsonPatchDocument<UpdateParkingSpaceDto>, JsonPatchDocument<ParkingSpace>>();
            CreateMap<Operation<UpdateParkingSpaceDto>, Operation<ParkingSpace>>();

            CreateMap<ParkingSpaceType, ParkingSpaceTypeDto>();
            CreateMap<AddParkingSpaceTypeDto, ParkingSpaceType>()
                .ForMember(e => e.Id, opts => opts.Ignore())
                .ForMember(e => e.RowVersion, opts => opts.Ignore());
        }

        private void CreateRentalAgreementMappings()
        {
            CreateMap<RentalAgreement, RentalAgreementDto>();
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

            CreateMap<RentalAgreementType, RentalAgreementTypeDto>();
            CreateMap<AddRentalAgreementTypeDto, RentalAgreementType>()
                .ForMember(e => e.Id, opts => opts.Ignore())
                .ForMember(e => e.RowVersion, opts => opts.Ignore());
        }
    }
}