using AutoMapper;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Core.Utils;
using PetSpa.ModelViews.OrderDetailModelViews;
using PetSpa.ModelViews.PackageModelViews;
using PetSpa.ModelViews.PackageServiceModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.Services.Mapper
{
    public class OrderDetailMapper:Profile
    {
        public OrderDetailMapper() 
        {
            // Mapping từ OrderDetail sang GETPackageModelView
            CreateMap<OrdersDetails, GETOrderDetailModelView>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.OrderID, opt => opt.MapFrom(src => src.OrderID))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price));


            CreateMap<OrdersDetails, GETOrderDetailModelView>()
            .ForMember(dest => dest.CreatedTime, opt => opt.MapFrom(src => src.CreatedTime))
            .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
            .ForMember(dest => dest.LastUpdatedTime, opt => opt.MapFrom(src => src.LastUpdatedTime))
            .ForMember(dest => dest.LastUpdatedBy, opt => opt.MapFrom(src => src.LastUpdatedBy))
            .ForMember(dest => dest.DeletedTime, opt => opt.MapFrom(src => src.DeletedTime))
            .ForMember(dest => dest.DeletedBy, opt => opt.MapFrom(src => src.DeletedBy));

            // Mapping từ PUTOrderDetailModelView sang Packages
            CreateMap<PUTOrderDetailModelView, OrdersDetails>()
           .ForMember(dest => dest.LastUpdatedTime, opt => opt.Ignore()) // Không map thuộc tính này, sẽ được cập nhật thủ công
           .ForMember(dest => dest.LastUpdatedBy, opt => opt.Ignore());


        }
    }
}
