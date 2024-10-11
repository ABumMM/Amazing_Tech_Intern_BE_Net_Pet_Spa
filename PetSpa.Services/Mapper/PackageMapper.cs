using AutoMapper;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.ModelViews.PackageModelViews;
using PetSpa.ModelViews.PackageServiceModelViews;
namespace PetSpa.Services.Mapper
{
    public class PackageMapper:Profile
    {
        public PackageMapper() 
        {
            // Mapping từ Packages sang GETPackageModelView
            CreateMap<Packages, GETPackageModelView>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
            .ForMember(dest => dest.Information, opt => opt.MapFrom(src => src.Information))
            .ForMember(dest => dest.Experiences, opt => opt.MapFrom(src => src.Experiences));

            CreateMap<Packages, GETPackageModelView>()
            .ForMember(dest => dest.CreatedTime, opt => opt.MapFrom(src => src.CreatedTime))
            .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
            .ForMember(dest => dest.LastUpdatedTime, opt => opt.MapFrom(src => src.LastUpdatedTime))
            .ForMember(dest => dest.LastUpdatedBy, opt => opt.MapFrom(src => src.LastUpdatedBy))
            .ForMember(dest => dest.DeletedTime, opt => opt.MapFrom(src => src.DeletedTime))
            .ForMember(dest => dest.DeletedBy, opt => opt.MapFrom(src => src.DeletedBy));

            // Mapping từ PackageServices sang GETPackageServiceModelView
            CreateMap<PackageServiceEntity, GETPackageServiceModelView>()
                .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.ServicesEntity != null ? src.ServicesEntity.Name : string.Empty));

            // Mapping từ PUTPackageModelView sang Packages
            CreateMap<PUTPackageModelView, Packages>()
           .ForMember(dest => dest.LastUpdatedTime, opt => opt.Ignore()) // Không map thuộc tính này, sẽ được cập nhật thủ công
           .ForMember(dest => dest.LastUpdatedBy, opt => opt.Ignore());

        }
    }
}
