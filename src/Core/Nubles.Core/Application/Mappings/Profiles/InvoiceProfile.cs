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

        public override void CreateAddMappings()
        {
            CreateMap<AddInvoiceStatusDto, InvoiceStatus>()
                .ForMember(e => e.Id, opts => opts.Ignore())
                .ForMember(e => e.RowVersion, opts => opts.Ignore());
        }

        public override void CreateGetMappings()
        {
            CreateMap<InvoiceStatus, InvoiceStatusDto>();
        }

        public override void CreateUpdateMappings()
        {
            // TODO: create Invoice mappings
        }
    }
}