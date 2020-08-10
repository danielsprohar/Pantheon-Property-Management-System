using System.ComponentModel.DataAnnotations;

namespace Nubles.Core.Application.Dto.Writes
{
    public class AddCustomerVehicleDto
    {
        [Required]
        public int? Customerid { get; set; }

        [Required]
        public int? Year { get; set; }

        [Required]
        public string Make { get; set; }

        [Required]
        public string Model { get; set; }

        [Required]
        public string Color { get; set; }

        [Required]
        public string LicensePlateState { get; set; }

        [Required]
        public string LicensePlateNumber { get; set; }
    }
}