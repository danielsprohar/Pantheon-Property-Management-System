using Pantheon.Core.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Pantheon.Core.Application.Dto.Writes
{
    public class AddParkingSpaceTypeDto
    {
        [Required]
        [StringLength((int)ParkingSpaceType.DbColumnLength.SpaceType)]
        public string SpaceType { get; set; }
    }
}