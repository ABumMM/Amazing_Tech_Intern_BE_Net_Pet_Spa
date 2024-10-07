using AutoMapper;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.ModelViews.UserModelViews;

namespace PetSpa.Services.Mapper
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<ApplicationUser, GETUserModelView>()
                .ForMember(dest => dest.UserInfo, opt => opt.MapFrom(src => src.UserInfo))
                .ForMember(dest => dest.RoleName, opt => opt.Ignore());

            CreateMap<PUTUserModelView, ApplicationUser>()
                .ForMember(dest => dest.UserInfo, opt => opt.MapFrom(src => src.UserInfo))
                .ForMember(dest => dest.Id, opt => opt.Ignore());

        }
    }
}
