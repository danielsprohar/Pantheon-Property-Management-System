using Nubles.Core.Domain.Base;

namespace Nubles.Core.Domain.Models
{
    public class ParkingSpaceType : Entity
    {
        public enum DBColumnLength
        {
            SpaceType = 32
        }

        public static class Type
        {
            public const string RV = "rv_space";
            public const string MobileHome = "mobile_home_space";
            public const string House = "house";
        }

        public string SpaceType { get; set; }
    }
}