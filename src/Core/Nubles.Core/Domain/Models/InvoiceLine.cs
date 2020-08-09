using Nubles.Core.Domain.Base;

namespace Nubles.Core.Domain.Models
{
    public class InvoiceLine : Entity
    {
        public enum DbColumnLength
        {
            Description = 256
        }

        public static class DecimalPrecision
        {
            public const string Precision = "DECIMAL(10, 2)";
        }

        public int Quantity { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }

        #region Navigation Properties

        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; }

        public int ParkingSpaceId { get; set; }
        public ParkingSpace ParkingSpace { get; set; }

        #endregion Navigation Properties
    }
}