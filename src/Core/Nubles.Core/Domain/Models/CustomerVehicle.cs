using Nubles.Core.Domain.Base;

namespace Nubles.Core.Domain.Models
{
    public class CustomerVehicle : Entity
    {
        public enum DBColumnLength
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

        #region Navigation Properties

        public int OwnerId { get; set; }
        public Customer Owner { get; set; }

        #endregion Navigation Properties
    }
}