using Nubles.Core.Domain.Base;

namespace Nubles.Core.Domain.Models
{
    public class InvoiceStatus : Entity
    {
        public enum DBColumnLength
        {
            Description = 32
        }

        public static class Status
        {
            public const string AwaitingPayment = "awaiting payment";
            public const string BadDebt = "bad debt";
            public const string Draft = "draft";
            public const string Paid = "paid";
            public const string PastDue = "past due";
            public const string Partial = "partial";
            public const string Void = "void";
        }

        public string Description { get; set; }
    }
}