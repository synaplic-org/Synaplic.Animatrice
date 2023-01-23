using AutoMapper;
using Synaplic.Inventory.Infrastructure.Models.Identity;
using Synaplic.Inventory.Transfer.Responses.Identity;

namespace Synaplic.Inventory.Infrastructure.Mappings
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleResponse, UniRole>().ReverseMap();
        }
    }
}