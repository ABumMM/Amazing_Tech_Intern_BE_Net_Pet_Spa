using AutoMapper;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.ModelViews.PetsModelViews;
using PetSpa.ModelViews.UserModelViews;

namespace PetSpa.Services.Mapper
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            // Ánh xạ từ ApplicationUser sang GETUserModelView
            CreateMap<ApplicationUser, GETUserModelView>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.UserInfo, opt => opt.MapFrom(src => src.UserInfo))
                .ForMember(dest => dest.RoleName, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.CreatedTime, opt => opt.MapFrom(src => src.CreatedTime))
                .ForMember(dest => dest.LastUpdatedBy, opt => opt.MapFrom(src => src.LastUpdatedBy))
                .ForMember(dest => dest.LastUpdatedTime, opt => opt.MapFrom(src => src.LastUpdatedTime))
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedTime, opt => opt.Ignore())
                .ForMember(dest => dest.Pets, opt => opt.MapFrom(src => src.Pets));

            // Ánh xạ từ PUTUserModelView sang ApplicationUser
            CreateMap<PUTUserModelView, ApplicationUser>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.UserInfo, opt => opt.MapFrom(src => src.UserInfo));

            // Ánh xạ từ PUTuserforcustomer sang ApplicationUser
            CreateMap<PUTuserforcustomer, ApplicationUser>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.UserInfo, opt => opt.MapFrom(src => src.UserInfo));

            // Ánh xạ từ UserInfo sang UserInfoModelView và ngược lại
            CreateMap<UserInfo, GETUserInfoModelView>().ReverseMap();

            // Ánh xạ từ Pets sang PetModelView và ngược lại
            CreateMap<Pets, GETPetsModelView>().ReverseMap();
        }
    }
}
