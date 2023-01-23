using AutoMapper;
using Synaplic.Inventory.Transfer.Requests.Identity;
using Synaplic.Inventory.Transfer.Responses.Identity;

namespace Synaplic.Inventory.Client.Infrastructure.Mappings
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<PermissionResponse, PermissionRequest>().ReverseMap();
            CreateMap<RoleClaimResponse, RoleClaimRequest>().ReverseMap();
        }
    }
}