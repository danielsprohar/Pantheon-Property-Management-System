﻿using Pantheon.Core.Application.Dto.Reads;
using Pantheon.Core.Application.Dto.Writes;
using Pantheon.Core.Application.Mappings.Resolvers;
using Pantheon.Core.Domain.Models;
using System.Globalization;

namespace Pantheon.Core.Application.Mappings.Profiles
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
                .ForMember(e => e.EmployeeId, opts => opts.MapFrom(dto => dto.EmployeeId))
                .ForMember(e => e.ModifiedBy, opts => opts.MapFrom(dto => dto.EmployeeId))
                .ForMember(e => e.CreatedOn, opts => opts.Ignore())
                .ForMember(e => e.Id, opts => opts.Ignore())
                .ForMember(e => e.InvoicePayments, opts => opts.Ignore())
                .ForMember(e => e.InvoiceStatus, opts => opts.Ignore())
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
            CreateMap<Invoice, InvoiceDto>()
                .ForMember(e => e.BillingPeriodEnd,
                            opts => opts.MapFrom(r =>
                                r.BillingPeriodEnd.ToString("d", DateTimeFormatInfo.InvariantInfo)))
                .ForMember(e => e.BillingPeriodStart,
                            opts => opts.MapFrom(r =>
                                r.BillingPeriodStart.ToString("d", DateTimeFormatInfo.InvariantInfo)))
                .ForMember(e => e.DueDate,
                            opts => opts.MapFrom(r =>
                                r.DueDate.ToString("d", DateTimeFormatInfo.InvariantInfo)));
        }

        protected override void CreateUpdateMappings()
        {
            CreateMap<UpdateInvoiceDto, Invoice>()
                .ForMember(e => e.BillingPeriodEnd, opts => opts.Ignore())
                .ForMember(e => e.BillingPeriodStart, opts => opts.Ignore())
                .ForMember(e => e.EmployeeId, opts => opts.Ignore())
                .ForMember(e => e.CreatedOn, opts => opts.Ignore())
                .ForMember(e => e.DueDate, opts => opts.Ignore())
                .ForMember(e => e.Id, opts => opts.Ignore())
                .ForMember(e => e.InvoiceLines, opts => opts.Ignore())
                .ForMember(e => e.InvoicePayments, opts => opts.Ignore())
                .ForMember(e => e.InvoiceStatus, opts => opts.Ignore())
                .ForMember(e => e.RentalAgreement, opts => opts.Ignore())
                .ForMember(e => e.RentalAgreementId, opts => opts.Ignore())
                .ForMember(e => e.RowVersion, opts => opts.Ignore())
                .ForMember(e => e.ModifiedBy, opts => opts.MapFrom(dto => dto.EmployeeId))
                .ForMember(e => e.ModifiedOn, opts => opts.MapFrom<UtcNowDateResolver>());
        }
    }
}