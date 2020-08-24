using System;
using System.ComponentModel.DataAnnotations;

namespace Pantheon.Core.Application.Dto.Writes
{
    ///<summary>
    /// Updates a <c>ParkingSpace</c> via HTTP PUT
    ///</summary>
    public class UpdateParkingSpaceDto : PatchParkingSpaceDto
    {
        [Required]
        public Guid EmployeeId { get; set; }
    }
}