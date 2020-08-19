using Pantheon.Core.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Pantheon.Core.Application.Dto.Writes
{
    public class UpdateInvoiceDto
    {
        public int InvoiceStatusId { get; set; }

        [StringLength((int)Invoice.DbColumnLength.Comments)]
        public string Comments { get; set; }
    }
}