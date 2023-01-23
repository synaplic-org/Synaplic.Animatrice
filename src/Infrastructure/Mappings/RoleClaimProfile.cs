using AutoMapper;
using Synaplic.Inventory.Infrastructure.Models.Identity;
using Synaplic.Inventory.Transfer.Requests.Identity;
using Synaplic.Inventory.Transfer.Responses.Identity;

namespace Synaplic.Inventory.Infrastructure.Mappings
{
    public class RoleClaimProfile : Profile
    {
        public RoleClaimProfile()
        {
            CreateMap<RoleClaimResponse, UniRoleClaim>()
                .ForMember(nameof(UniRoleClaim.ClaimType), opt => opt.MapFrom(c => c.Type))
                .ForMember(nameof(UniRoleClaim.ClaimValue), opt => opt.MapFrom(c => c.Value))
                .ReverseMap();

            CreateMap<RoleClaimRequest, UniRoleClaim>()
                .ForMember(nameof(UniRoleClaim.ClaimType), opt => opt.MapFrom(c => c.Type))
                .ForMember(nameof(UniRoleClaim.ClaimValue), opt => opt.MapFrom(c => c.Value))
                .ReverseMap();
        }
    }
}