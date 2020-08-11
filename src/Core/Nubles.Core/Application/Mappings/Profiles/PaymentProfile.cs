using Nubles.Core.Application.Dto.Reads;
using Nubles.Core.Application.Dto.Writes;
using Nubles.Core.Domain.Models;

namespace Nubles.Core.Application.Mappings.Profiles
{
    public class PaymentProfile : EntityProfile
    {
        public PaymentProfile()
        {
            CreateAddMappings();
            CreateGetMappings();
            CreateUpdateMappings();
        }

        protected override void CreateAddMappings()
        {
            CreateMap<AddPaymentMethodDto, PaymentMethod>()
                .ForMember(e => e.Id, opts => opts.Ignore())
                .ForMember(e => e.RowVersion, opts => opts.Ignore());
        }

        protected override void CreateGetMappings()
        {
            CreateMap<PaymentMethod, PaymentMethodDto>();
        }

        protected override void CreateUpdateMappings()
        {
            // TODO: create Payment mappings
        }
    }
}