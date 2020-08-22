using Pantheon.Core.Application.Dto.Reads;
using Pantheon.Core.Application.Dto.Writes;
using Pantheon.Core.Domain.Models;

namespace Pantheon.Core.Application.Mappings.Profiles
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
            CreateMap<AddPaymentDto, Payment>()
                .ForMember(e => e.EmployeeId, opts => opts.MapFrom(dto => dto.EmployeeId))
                .ForMember(e => e.ModifiedBy, opts => opts.MapFrom(dto => dto.EmployeeId))
                .ForMember(e => e.CreatedOn, opts => opts.Ignore())
                .ForMember(e => e.Customer, opts => opts.Ignore())
                .ForMember(e => e.Id, opts => opts.Ignore())
                .ForMember(e => e.InvoicePayments, opts => opts.Ignore())
                .ForMember(e => e.ModifiedOn, opts => opts.Ignore())
                .ForMember(e => e.PaymentMethod, opts => opts.Ignore())
                .ForMember(e => e.RowVersion, opts => opts.Ignore());

            CreateMap<AddPaymentMethodDto, PaymentMethod>()
                .ForMember(e => e.Id, opts => opts.Ignore())
                .ForMember(e => e.RowVersion, opts => opts.Ignore());
        }

        protected override void CreateGetMappings()
        {
            CreateMap<Payment, PaymentDto>();
            CreateMap<PaymentMethod, PaymentMethodDto>();
        }

        protected override void CreateUpdateMappings()
        {
        }
    }
}