namespace Pantheon.Core.Application.Dto.Reads
{
    public class ParkingSpaceDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal RecurringRate { get; set; }
        public bool? IsAvailable { get; set; }
        public int? Amps { get; set; }
        public string Comments { get; set; }
        public ParkingSpaceTypeDto ParkingSpaceType { get; set; }
    }
}