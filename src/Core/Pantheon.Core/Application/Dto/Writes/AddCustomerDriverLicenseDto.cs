using Pantheon.Core.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Pantheon.Core.Application.Dto.Writes
{
    public class AddCustomerDriverLicenseDto
    {
        [StringLength((int)CustomerDriverLicense.DBColumnLength.Number)]
        public string Number { get; set; }

        [StringLength((int)CustomerDriverLicense.DBColumnLength.State)]
        public string State { get; set; }

        [StringLength(4096)]
        public string PhotoUrl { get; set; }
    }
}