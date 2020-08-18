namespace Vulcan.Web.Options
{
    // Options Pattern
    // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options?view=aspnetcore-3.1
    public class HermesApiOptions
    {
        public const string HermesApi = "HermesApi";
        public string BaseAddress { get; set; }
        public HermesApiVersion Version { get; set; }
        public HermesApiResourcePath ResourcePath { get; set; }
    }

    public class HermesApiVersion
    {
        public string V1 { get; set; }
    }

    public class HermesApiResourcePath
    {
        public string Invoices { get; set; }
        public string InvoiceStatuses { get; set; }
        public string ParkingSpaces { get; set; }
        public string ParkingSpaceTypes { get; set; }
        public string PaymentMethods { get; set; }
        public string Payments { get; set; }
        public string RentalAgreements { get; set; }
        public string RentalAgreementTypes { get; set; }
    }
}