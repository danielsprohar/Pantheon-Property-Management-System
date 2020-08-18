using Pantheon.Core.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Pantheon.Core.Application.Dto.Writes
{
    public class AddInvoiceStatusDto
    {
        [Required]
        [StringLength((int)InvoiceStatus.DbColumnLength.Description)]
        public string Description { get; set; }
    }
}