using AutoMapper;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.ModelViews.UserModelViews;
using PetSpa.ModelViews.PetsModelViews;

namespace PetSpa.Services.Mapper
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            // Ánh xạ từ ApplicationUser sang GETUserModelView
            CreateMap<ApplicationUser, GETUserModelView>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.UserInfo != null ? src.UserInfo.FullName : string.Empty))
                .ForMember(dest => dest.RoleName, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.LastUpdatedBy, opt => opt.MapFrom(src => src.LastUpdatedBy))
                .ForMember(dest => dest.DeletedBy, opt => opt.MapFrom(src => src.DeletedBy))
                .ForMember(dest => dest.CreatedTime, opt => opt.MapFrom(src => src.CreatedTime))
                .ForMember(dest => dest.LastUpdatedTime, opt => opt.MapFrom(src => src.LastUpdatedTime))
                .ForMember(dest => dest.DeletedTime, opt => opt.MapFrom(src => src.DeletedTime))
                // Ánh xạ danh sách Pets
                .ForMember(dest => dest.Pets, opt => opt.MapFrom(src => src.Pets));

            // Ánh xạ từ PUTUserModelView sang ApplicationUser
            CreateMap<PUTUserModelView, ApplicationUser>()
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedTime, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedTime, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedTime, opt => opt.Ignore());
        }
    }
}
