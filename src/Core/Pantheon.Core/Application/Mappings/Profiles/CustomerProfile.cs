using Pantheon.Core.Application.Dto.Reads;
using Pantheon.Core.Application.Dto.Writes;
using Pantheon.Core.Application.Mappings.Resolvers;
using Pantheon.Core.Domain.Models;

namespace Pantheon.Core.Application.Mappings.Profiles
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
                .ForMember(e => e.EmployeeId, opts => opts.MapFrom(dto => dto.EmployeeId))
                .ForMember(e => e.ModifiedBy, opts => opts.MapFrom(dto => dto.EmployeeId))
                .ForMember(e => e.IsActive, opts => opts.Ignore())
                .ForMember(e => e.Vehicles, opts => opts.Ignore())
                .ForMember(e => e.CustomerRentalAgreements, opts => opts.Ignore())
                .ForMember(e => e.Payments, opts => opts.Ignore())
                .ForMember(e => e.CreatedOn, opts => opts.Ignore())
                .ForMember(e => e.ModifiedOn, opts => opts.Ignore())
                .ForMember(e => e.Id, opts => opts.Ignore())
                .ForMember(e => e.RowVersion, opts => opts.Ignore())
                .ForMember(e => e.NormalizedEmail,
                    opts => opts.MapFrom(valueResolver => valueResolver.Email.ToUpperInvariant()));

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
            CreateMap<UpdateCustomerDto, Customer>()
                .ForMember(e => e.IsActive, opts => opts.Ignore())
                .ForMember(e => e.Vehicles, opts => opts.Ignore())
                .ForMember(e => e.CustomerRentalAgreements, opts => opts.Ignore())
                .ForMember(e => e.Payments, opts => opts.Ignore())
                .ForMember(e => e.EmployeeId, opts => opts.Ignore())
                .ForMember(e => e.CreatedOn, opts => opts.Ignore())
                .ForMember(e => e.Id, opts => opts.Ignore())
                .ForMember(e => e.RowVersion, opts => opts.Ignore())
                .ForMember(e => e.NormalizedEmail,
                    opts => opts.MapFrom(valueResolver => valueResolver.Email.ToUpperInvariant()))
                .ForMember(e => e.ModifiedBy, opts => opts.MapFrom(dto => dto.EmployeeId))
                .ForMember(e => e.ModifiedOn, opts => opts.MapFrom<UtcNowDateResolver>());
        }
    }
}