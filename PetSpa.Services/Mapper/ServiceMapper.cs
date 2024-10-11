using AutoMapper;
using PetSpa.ModelViews.ServiceModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicesEntity = PetSpa.Contract.Repositories.Entity.Services;
namespace PetSpa.Services.Mapper
{
    public class ServiceMapper : Profile
    {
        public ServiceMapper() {
            // Map ServiceCreateModel --> ServicesEntity 
            CreateMap<ServiceCreateModel, ServicesEntity>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));
            
            // Map ServiceCreateModel --> ServicesEntity 
            CreateMap<ServicesEntity, ServiceResposeModel>();
            
            // Map ServiceUpdateModel --> ServiceEntity
            CreateMap<ServiceUpdateModel, ServicesEntity>()
            .ForMember(dest => dest.PackageServices, opt => opt.Ignore())  // Ignore navigation property
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())        // Ignore internal metadata
            .ForMember(dest => dest.LastUpdatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedTime, opt => opt.Ignore())      // Preserve the original timestamps
            .ForMember(dest => dest.LastUpdatedTime, opt => opt.MapFrom(src => DateTimeOffset.Now)); // Update LastUpdatedTime
        }

    }
}
