using AutoMapper;
using Nubles.Core.Application.Dto.Reads;
using Nubles.Core.Application.Dto.Writes;
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
            CreateMap<AddCustomerDto, Customer>();
            CreateMap<Customer, CustomerDto>();

            CreateMap<AddCustomerVehicleDto, CustomerVehicle>();
            CreateMap<CustomerVehicle, CustomerVehicleDto>();

            CreateMap<AddCustomerDriverLicenseDto, CustomerDriverLicense>();
            CreateMap<CustomerDriverLicense, CustomerDriverLicense>();
        }

        private void CreateParkingSpaceMappings()
        {
            CreateMap<AddParkingSpaceDto, ParkingSpace>();
            CreateMap<ParkingSpace, ParkingSpaceDto>();            

            CreateMap<AddParkingSpaceTypeDto, ParkingSpaceType>();
            CreateMap<ParkingSpaceType, ParkingSpaceTypeDto>();
        }

        private void CreateRentalAgreementMappings()
        {
            CreateMap<AddRentalAgreementDto, RentalAgreement>();
            CreateMap<RentalAgreement, RentalAgreementDto>();
        }
    }
}
