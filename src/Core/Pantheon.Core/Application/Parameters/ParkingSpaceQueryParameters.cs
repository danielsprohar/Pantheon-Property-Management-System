﻿namespace Nubles.Core.Application.Parameters
{
    public class ParkingSpaceQueryParameters : QueryParameters
    {
        public bool? IsAvailable { get; set; }

        public int? Amps { get; set; }
    }
}