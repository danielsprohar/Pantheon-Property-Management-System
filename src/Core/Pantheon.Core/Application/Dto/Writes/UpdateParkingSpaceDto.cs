﻿using Pantheon.Core.Domain.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Pantheon.Core.Application.Dto.Writes
{
    public class UpdateParkingSpaceDto
    {
        [StringLength((int)ParkingSpace.DbColumnLength.Name)]
        public string Name { get; set; }

        [StringLength((int)ParkingSpace.DbColumnLength.Description)]
        public string Description { get; set; }

        public decimal? RecurringRate { get; set; }

        public bool? IsAvailable { get; set; }

        public int? Amps { get; set; }

        [StringLength((int)ParkingSpace.DbColumnLength.Commments)]
        public string Comments { get; set; }

        public int? ParkingSpaceTypeId { get; set; }

        [Required]
        public Guid EmployeeId { get; set; }
    }
}