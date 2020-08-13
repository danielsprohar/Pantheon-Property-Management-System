using Nubles.Core.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Nubles.Core.Application.Dto.Writes
{
    public class AddInvoiceLineDto
    {
        [Required]
        public int? Quantity { get; set; }

        [Required]
        [StringLength((int)InvoiceLine.DbColumnLength.Description)]
        public string Description { get; set; }

        [Required]
        public decimal? Price { get; set; }

        [Required]
        public decimal? Total { get; set; }

        public int InvoiceId { get; set; }

        [Required]
        public int? ParkingSpaceId { get; set; }
    }
}