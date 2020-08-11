using Nubles.Core.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Nubles.Core.Application.Dto.Writes
{
    public class AddInvoiceStatusDto
    {
        [Required]
        [StringLength((int)InvoiceStatus.DbColumnLength.Description)]
        public string Description { get; set; }
    }
}