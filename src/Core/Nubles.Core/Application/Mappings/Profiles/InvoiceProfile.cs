using Nubles.Core.Application.Dto.Reads;
using Nubles.Core.Application.Dto.Writes;
using Nubles.Core.Domain.Models;

namespace Nubles.Core.Application.Mappings.Profiles
{
    public class InvoiceProfile : EntityProfile
    {
        public InvoiceProfile()
        {
            CreateAddMappings();
            CreateGetMappings();
            CreateUpdateMappings();
        }

        protected override void CreateAddMappings()
        {
            CreateMap<AddInvoiceDto, Invoice>()
                .ForMember(e => e.CreatedBy, opts => opts.Ignore())
                .ForMember(e => e.CreatedOn, opts => opts.Ignore())
                .ForMember(e => e.Id, opts => opts.Ignore())
                .ForMember(e => e.InvoicePayments, opts => opts.Ignore())
                .ForMember(e => e.InvoiceStatus, opts => opts.Ignore())
                .ForMember(e => e.ModifiedBy, opts => opts.Ignore())
                .ForMember(e => e.ModifiedOn, opts => opts.Ignore())
                .ForMember(e => e.RentalAgreement, opts => opts.Ignore())
                .ForMember(e => e.RowVersion, opts => opts.Ignore());

            CreateMap<AddInvoiceStatusDto, InvoiceStatus>()
                .ForMember(e => e.Id, opts => opts.Ignore())
                .ForMember(e => e.RowVersion, opts => opts.Ignore());

            CreateMap<AddInvoiceLineDto, InvoiceLine>()
                .ForMember(e => e.Invoice, opts => opts.Ignore())
                .ForMember(e => e.ParkingSpace, opts => opts.Ignore());
        }

        protected override void CreateGetMappings()
        {
            CreateMap<InvoiceStatus, InvoiceStatusDto>();
            CreateMap<InvoiceLine, InvoiceLineDto>();
            CreateMap<Invoice, InvoiceDto>();
        }

        protected override void CreateUpdateMappings()
        {
            CreateMap<UpdateInvoiceDto, Invoice>()
                .ForMember(e => e.BillingPeriodEnd, opts => opts.Ignore())
                .ForMember(e => e.BillingPeriodStart, opts => opts.Ignore())
                .ForMember(e => e.CreatedBy, opts => opts.Ignore())
                .ForMember(e => e.CreatedOn, opts => opts.Ignore())
                .ForMember(e => e.DueDate, opts => opts.Ignore())
                .ForMember(e => e.Id, opts => opts.Ignore())
                .ForMember(e => e.InvoiceLines, opts => opts.Ignore())
                .ForMember(e => e.InvoicePayments, opts => opts.Ignore())
                .ForMember(e => e.InvoiceStatus, opts => opts.Ignore())
                .ForMember(e => e.ModifiedBy, opts => opts.Ignore())
                .ForMember(e => e.ModifiedOn, opts => opts.Ignore())
                .ForMember(e => e.RentalAgreement, opts => opts.Ignore())
                .ForMember(e => e.RentalAgreementId, opts => opts.Ignore())
                .ForMember(e => e.RowVersion, opts => opts.Ignore());
        }
    }
}