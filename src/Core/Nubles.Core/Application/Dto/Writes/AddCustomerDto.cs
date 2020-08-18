using Pantheon.Core.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Pantheon.Core.Application.Dto.Writes
{
    public class AddCustomerDto
    {
        [Required]
        [StringLength((int)Customer.DbColumnLength.Name)]
        public string FirstName { get; set; }

        [StringLength((int)Customer.DbColumnLength.Name)]
        public string MiddleName { get; set; }

        [Required]
        [StringLength((int)Customer.DbColumnLength.Name)]
        public string LastName { get; set; }

        [Required]
        public char Gender { get; set; }

        [Phone]
        [Required]
        [StringLength((int)Customer.DbColumnLength.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        [StringLength((int)Customer.DbColumnLength.Email)]
        public string Email { get; set; }

        #region Owned entity attributes

        public AddCustomerDriverLicenseDto DriverLicense { get; set; }

        public AddCustomerVehicleDto Vehicle { get; set; }

        #endregion Owned entity attributes
    }
}