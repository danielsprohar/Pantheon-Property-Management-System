using Nubles.Core.Domain.Base;

namespace Nubles.Core.Domain.Models
{
    public class ParkingSpace : AuditableEntity
    {
        public enum AmpCapacity
        {
            Thirty = 30,
            Fifty = 50
        }

        public enum DBColumnLength
        {
            Name = 4,
            Description = 32,
            Commments = 2048
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Rate { get; set; }

        public bool? IsAvailable { get; set; }

        public int? Amps { get; set; }

        public string Comments { get; set; }

        #region Navigation properties

        public int ParkingSpaceTypeId { get; set; }
        public ParkingSpaceType ParkingSpaceType { get; set; }

        #endregion Navigation properties
    }
}