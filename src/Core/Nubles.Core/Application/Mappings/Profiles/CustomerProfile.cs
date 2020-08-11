using Nubles.Core.Application.Dto.Reads;
using Nubles.Core.Application.Dto.Writes;
using Nubles.Core.Application.Mappings.Resolvers;
using Nubles.Core.Domain.Models;

namespace Nubles.Core.Application.Mappings.Profiles
{
    public class CustomerProfile : EntityProfile
    {
        public CustomerProfile()
        {
            CreateAddMappings();
            CreateGetMappings();
            CreateUpdateMappings();
        }

        protected override void CreateAddMappings()
        {
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

            CreateMap<AddCustomerVehicleDto, CustomerVehicle>()
                .ForMember(e => e.Id, opts => opts.Ignore())
                .ForMember(e => e.RowVersion, opts => opts.Ignore())
                .ForMember(e => e.Customer, opts => opts.Ignore());

            CreateMap<AddCustomerDriverLicenseDto, CustomerDriverLicense>();
        }

        protected override void CreateGetMappings()
        {
            CreateMap<Customer, CustomerDto>()
                .ForMember(e => e.RentalAgreements, opts => opts.Ignore());

            CreateMap<CustomerVehicle, CustomerVehicleDto>();

            CreateMap<CustomerDriverLicense, CustomerDriverLicenseDto>();
        }

        protected override void CreateUpdateMappings()
        {
            // TODO: create mappings
        }
    }
}