using Nubles.Core.Domain.Base;

namespace Nubles.Core.Domain.Models
{
    public class RentalAgreementType : Entity
    {
        public enum DBColumnLength
        {
            Description = 16
        }

        public static class Type
        {
            public const string Daily = "daily";
            public const string Monthly = "monthly";
            public const string Weekly = "weekly";
        }

        public string AgreementType { get; set; }
    }
}