using Nubles.Core.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Nubles.Core.Application.Dto.Writes
{
    public class UpdateCustomerDto
    {
        [StringLength((int)Customer.DbColumnLength.Name)]
        public string FirstName { get; set; }

        [StringLength((int)Customer.DbColumnLength.Name)]
        public string MiddleName { get; set; }

        [StringLength((int)Customer.DbColumnLength.Name)]
        public string LastName { get; set; }

        public char Gender { get; set; }

        [Phone]
        [StringLength((int)Customer.DbColumnLength.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        [StringLength((int)Customer.DbColumnLength.Email)]
        public string Email { get; set; }

        public bool? IsActive { get; set; }
    }
}