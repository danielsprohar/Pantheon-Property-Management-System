using Nubles.Core.Domain.Base;

namespace Nubles.Core.Domain.Models
{
    public class PaymentMethod : Entity
    {
        public enum DbColumnLength
        {
            PaymentMethod = 32
        }

        public static class Type
        {
            public const string Cash = "cash";
            public const string Check = "check";
            public const string Credit = "credit";
            public const string Debit = "debit";
            public const string MoneyOrder = "money_order";
        }

        public string Method { get; set; }
    }
}