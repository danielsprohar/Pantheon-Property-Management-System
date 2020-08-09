using Nubles.Core.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Nubles.Core.Application.Dto.Writes
{
    public class AddCustomerDto
    {
        [Required]
        [StringLength((int)Customer.DBColumnLength.Name)]
        public string FirstName { get; set; }

        [StringLength((int)Customer.DBColumnLength.Name)]
        public string MiddleName { get; set; }

        [Required]
        [StringLength((int)Customer.DBColumnLength.Name)]
        public string LastName { get; set; }

        [Required]
        public char Gender { get; set; }

        [Phone]
        [Required]
        [StringLength((int)Customer.DBColumnLength.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        [StringLength((int)Customer.DBColumnLength.Email)]
        public string Email { get; set; }

        #region Owned entity attributes

        [StringLength((int)CustomerDriverLicense.DBColumnLength.Number)]
        public string DLNumber { get; set; }

        [StringLength((int)CustomerDriverLicense.DBColumnLength.State)]
        public string DLState { get; set; }

        [StringLength(4096)]
        public string DLPhotoUrl { get; set; }

        #endregion Owned entity attributes
    }
}