using Nubles.Core.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Nubles.Core.Application.Dto.Writes
{
    public class AddParkingSpaceTypeDto
    {
        [Required]
        [StringLength((int)ParkingSpaceType.DBColumnLength.SpaceType)]
        public string SpaceType { get; set; }
    }
}