using Nubles.Core.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Nubles.Core.Application.Dto.Writes
{
    public class AddRentalAgreementTypeDto
    {
        [Required]
        [StringLength((int)RentalAgreementType.DbColumnLength.AgreementType)]
        public string AgreementType { get; set; }
    }
}