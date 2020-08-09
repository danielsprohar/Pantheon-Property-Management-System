namespace Nubles.Core.Application.Dto.Writes
{
    public class AddCustomerVehicleDto
    {
        public int Year { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public string Color { get; set; }

        public string LicensePlateState { get; set; }

        public string LicensePlateNumber { get; set; }
    }
}