using AutoMapper;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.ModelViews.AuthModelViews;

namespace PetSpa.Services.Mapper
{
    public class AuthMapping : Profile
    {
        public AuthMapping()
        {
            // Map từ SignUpAuthModelView sang ApplicationUser
            CreateMap<SignUpAuthModelView, ApplicationUser>()
                .ForMember(dest => dest.UserInfo, opt => opt.MapFrom(src => new UserInfo { FullName = src.FullName }));
        }
    }
}
