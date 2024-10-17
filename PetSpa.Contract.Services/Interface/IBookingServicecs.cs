using PetSpa.Core.Base;
using PetSpa.ModelViews.BookingModelViews;

namespace PetSpa.Contract.Services.Interface
{
    public interface IBookingServicecs
    {
        Task<BasePaginatedList<GETBookingVM>> GetAll(int pageNumber, int pageSize);
        Task<GETBookingVM?> GetById(string id);
        Task<BasePaginatedList<GETBookingVM>> GetAllBookingByCustomer(int pageNumber, int pageSize);
        Task Add(POSTBookingVM bookingVM);
        Task Update( POSTBookingVM bookingVM, string id);
        Task CancelBooking(string bookingId);
    }
}
