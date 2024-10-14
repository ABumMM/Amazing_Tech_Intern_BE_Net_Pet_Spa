using AutoMapper;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.ModelViews.ReviewModelViews;

namespace PetSpa.Services.Mapper
{
    public class ReviewMapper:Profile
    {
        public ReviewMapper() 
        {
            // Mapping từ Packages sang GETPackageModelView
            CreateMap<Reviews, GETReviewModelViews>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.PackageID, opt => opt.MapFrom(src => src.PackageID))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CreatedTime, opt => opt.MapFrom(src => src.CreatedTime))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy));
        }
       
    }
}
