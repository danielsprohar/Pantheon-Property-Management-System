namespace Pantheon.Core.Application.Dto.Reads
{
    public class InvoiceLineDto
    {
        public int Quantity { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal Total { get; set; }

        public int InvoiceId { get; set; }

        public int ParkingSpaceId { get; set; }
    }
}