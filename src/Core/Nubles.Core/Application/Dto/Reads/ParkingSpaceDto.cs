namespace Nubles.Core.Application.Dto.Reads
{
    public class ParkingSpaceDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Rate { get; set; }
        public bool? IsAvailable { get; set; }
        public int? AmpCapacity { get; set; }
        public string Comments { get; set; }
        public ParkingSpaceTypeDto ParkingSpaceType { get; set; }
    }
}