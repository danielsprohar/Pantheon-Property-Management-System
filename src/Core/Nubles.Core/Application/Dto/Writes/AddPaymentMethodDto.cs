using Pantheon.Core.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Pantheon.Core.Application.Dto.Writes
{
    public class AddPaymentMethodDto
    {
        [Required]
        [StringLength((int)PaymentMethod.DbColumnLength.PaymentMethod)]
        public string Method { get; set; }
    }
}