using PetSpa.Contract.Repositories.Entity;
using PetSpa.ModelViews.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.Contract.Services.Interface
{
    public interface IBookingServicecs
    {
        //Task<IList<BookingResponseModel>> GetAll();
        //Task<BookingResponseModel?> GetById(object id);
        //Task Add(BookingResponseModel booking);
        //Task Update(BookingResponseModel booking);
        //Task Delete(object id);
        Task<IList<Bookings>> GetAll();
        Task<Bookings?> GetById(object id);
        Task Add(BookingResponseModel bookingVM);
        Task Update(Bookings booking);
        Task Delete(object id);
    }
}
