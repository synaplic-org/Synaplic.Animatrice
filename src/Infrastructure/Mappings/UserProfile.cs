using AutoMapper;
using Synaplic.Inventory.Infrastructure.Models.Identity;
using Synaplic.Inventory.Transfer.Responses.Identity;

namespace Synaplic.Inventory.Infrastructure.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserResponse, UniUser>().ReverseMap();
            //CreateMap<ChatUserResponse, UniUser>().ReverseMap()
            //    .ForMember(dest => dest.EmailAddress, source => source.MapFrom(source => source.Email)); //Specific Mapping
        }
    }
}