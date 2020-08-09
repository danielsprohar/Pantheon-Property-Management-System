using Nubles.Core.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Nubles.Core.Application.Dto.Writes
{
    public class AddParkingSpaceDto
    {
        [Required]
        [StringLength((int)ParkingSpace.DBColumnLength.Name)]
        public string Name { get; set; }

        [StringLength((int)ParkingSpace.DBColumnLength.Description)]
        public string Description { get; set; }

        [Required]
        public decimal RecurringRate { get; set; }

        public bool? IsAvailable { get; set; }

        public int? Amps { get; set; }

        [StringLength((int)ParkingSpace.DBColumnLength.Commments)]
        public string Comments { get; set; }

        [Required]
        public int UnitTypeId { get; set; }
    }
}