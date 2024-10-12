using AutoMapper;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.ModelViews.OrderDetailModelViews;
using PetSpa.ModelViews.OrderModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.Services.Mapper
{
    public class OrderMapper : Profile
    {
        public OrderMapper() {
            // Mapping từ Order sang GetOrderViewModel
            CreateMap<Orders, GetOrderViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod)) 
                .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.Total)) 
                .ForMember(dest => dest.IsPaid, opt => opt.MapFrom(src => src.IsPaid)) 
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy)) 
                .ForMember(dest => dest.CreatedTime, opt => opt.MapFrom(src => src.CreatedTime));

            // Mapping từ GETOrderDetailModelView sang OrderDetail 
            CreateMap<OrdersDetails, GETOrderDetailModelView>();

            // Mapping từ PostOrderViewModel sang Order
            CreateMap<PostOrderViewModel, Orders>()
                .ForMember(dest => dest.CreatedTime, opt => opt.MapFrom(src => src.CreatedTime)) 
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy)) 
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod)); 

            // Mapping từ PutOrderViewModel sang Order
            CreateMap<PutOrderViewModel, Orders>()
                .ForMember(dest => dest.LastUpdatedTime, opt => opt.MapFrom(src => src.LastUpdatedTime))
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod))
                .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.Total))
                .ForMember(dest => dest.LastUpdatedBy, opt => opt.MapFrom(src => src.LastUpdatedBy));


        }
    }
}
