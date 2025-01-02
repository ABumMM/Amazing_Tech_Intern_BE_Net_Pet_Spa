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
            // Map từ Orders sang GetOrderViewModel
            CreateMap<Orders, GetOrderViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.Total))
                .ForMember(dest => dest.IsPaid, opt => opt.MapFrom(src => src.IsPaid))
                .ForMember(dest => dest.CreatedTime, opt => opt.MapFrom(src => src.CreatedTime));

            // Map từ PostOrderViewModel sang Orders
            CreateMap<PostOrderViewModel, Orders>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CreatedTime, opt => opt.MapFrom(src => src.CreatedTime));

            // Map từ PutOrderViewModel sang Orders
            CreateMap<PutOrderViewModel, Orders>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.LastUpdatedTime, opt => opt.MapFrom(src => src.LastUpdatedTime));
        }
    }
}
