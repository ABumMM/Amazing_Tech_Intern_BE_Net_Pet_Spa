using AutoMapper;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.ModelViews.BookingPackageModelViews;

namespace PetSpa.Services.Mapper
{
    public class BookingPackageMapping : Profile
    {
        public BookingPackageMapping() {
            CreateMap<Booking_PackageVM, BookingPackage>()
               .ForMember(dest => dest.AddedDate, opt => opt.Ignore())
               .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));
            CreateMap<Bookings, GETBooking_PackageVM>()
                .ForMember(dest => dest.Packages, opt => opt.MapFrom(src => new List<PackageDTO>()))
                .ForMember(dest => dest.BookingId, opt => opt.MapFrom(src => src.Id));
            CreateMap<BookingPackage, PackageDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PackageId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Package != null ? src.Package.Name : string.Empty))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Package != null ? src.Package.Price : 0))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));
            CreateMap<BookingPackage, GETBooking_PackageVM>()
                .ForMember(dest => dest.Packages, opt => opt.MapFrom(src => src));
        }
    }
}
