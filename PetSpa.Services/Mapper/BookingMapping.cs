using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.ModelViews.BookingModelViews;

namespace PetSpa.Services.Mapper
{
    public class BookingMapping : Profile
    {
        public BookingMapping() {
            // Ánh xạ từ Bookings sang GETBookingVM
            CreateMap<Bookings, GETBookingVM>()
                .ForMember(dest => dest.LastUpdatedBy, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.LastUpdatedBy) ? null : src.LastUpdatedBy))
                .ForMember(dest => dest.LastUpdatedTime, opt => opt.MapFrom(src => src.LastUpdatedTime != DateTimeOffset.MinValue ? src.LastUpdatedTime : (DateTimeOffset?)null));

            // Ánh xạ từ POSTBookingVM sang Bookings
            CreateMap<POSTBookingVM, Bookings>()
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedTime, opt => opt.Ignore());
        }
    }
}
