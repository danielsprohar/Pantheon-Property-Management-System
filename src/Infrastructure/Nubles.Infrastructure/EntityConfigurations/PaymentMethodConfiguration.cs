using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nubles.Core.Domain.Models;

namespace Nubles.Infrastructure.EntityConfigurations
{
    internal class PaymentMethodConfiguration : EntityConfiguration<PaymentMethod>
    {
        public override void Configure(EntityTypeBuilder<PaymentMethod> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Method)
                .HasMaxLength((int)PaymentMethod.DbColumnLength.PaymentMethod)
                .IsRequired();

            builder.HasData(GetPaymentMethods());
        }

        private PaymentMethod[] GetPaymentMethods()
        {
            return new PaymentMethod[]
            {
                new PaymentMethod
                {
                    Id = 1,
                    Method = PaymentMethod.Type.Cash
                },
                new PaymentMethod
                {
                    Id = 2,
                    Method = PaymentMethod.Type.Check
                },
                new PaymentMethod
                {
                    Id = 3,
                    Method = PaymentMethod.Type.Credit
                },
                new PaymentMethod
                {
                    Id = 4,
                    Method = PaymentMethod.Type.Debit
                },
                new PaymentMethod
                {
                    Id = 5,
                    Method = PaymentMethod.Type.MoneyOrder
                }
            };
        }
    }
}