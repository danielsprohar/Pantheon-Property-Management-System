using System;
using System.Collections.Generic;

namespace Nubles.Core.Application.Dto.Reads
{
    public class CustomerDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public char Gender { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public DateTimeOffset ModifiedOn { get; set; }

        public CustomerDriverLicenseDto DriverLicense { get; set; }

        public ICollection<CustomerVehicleDto> Vehicles { get; set; } = new HashSet<CustomerVehicleDto>();

        public ICollection<RentalAgreementDto> RentalAgreements { get; set; } = new HashSet<RentalAgreementDto>();
    }
}