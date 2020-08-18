using Pantheon.Core.Domain.Base;

namespace Pantheon.Core.Domain.Models
{
    public class CustomerVehicle : Entity
    {
        public enum DbColumnLength
        {
            Make = 32,
            Model = 64,
            Color = 32,
            LicensePlateState = 32,
            LicensePlateNumber = 16
        }

        public int Year { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public string Color { get; set; }

        public string LicensePlateState { get; set; }

        public string LicensePlateNumber { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}