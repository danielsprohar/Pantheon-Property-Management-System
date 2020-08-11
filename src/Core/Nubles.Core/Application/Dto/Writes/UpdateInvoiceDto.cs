using Nubles.Core.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Nubles.Core.Application.Dto.Writes
{
    public class UpdateInvoiceDto
    {
        public int InvoiceStatusId { get; set; }

        [StringLength((int)Invoice.DbColumnLength.Comments)]
        public string Comments { get; set; }
    }
}