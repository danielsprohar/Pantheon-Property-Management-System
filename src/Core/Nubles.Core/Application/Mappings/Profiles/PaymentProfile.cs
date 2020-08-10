using Nubles.Core.Application.Dto.Reads;
using Nubles.Core.Application.Dto.Writes;
using Nubles.Core.Domain.Models;
using System;

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

        public override void CreateAddMappings()
        {
            CreateMap<AddPaymentMethodDto, PaymentMethod>()
                .ForMember(e => e.Id, opts => opts.Ignore())
                .ForMember(e => e.RowVersion, opts => opts.Ignore());
        }

        public override void CreateGetMappings()
        {
            CreateMap<PaymentMethod, PaymentMethodDto>();
        }

        public override void CreateUpdateMappings()
        {
            // TODO: create Payment mappings
        }
    }
}