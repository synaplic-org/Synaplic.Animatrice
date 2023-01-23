using AutoMapper;
using Synaplic.Inventory.Infrastructure.Models.Audit;
using Synaplic.Inventory.Transfer.Responses.Audit;

namespace Synaplic.Inventory.Infrastructure.Mappings
{
    public class AuditProfile : Profile
    {
        public AuditProfile()
        {
            CreateMap<AuditResponse, Audit>().ReverseMap();
        }
    }
}