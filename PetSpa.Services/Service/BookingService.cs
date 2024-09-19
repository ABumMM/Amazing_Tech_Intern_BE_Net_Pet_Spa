using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Repositories.IUOW;
using PetSpa.Contract.Services.Interface;
using PetSpa.ModelViews.ModelViews;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PetSpa.Services.Service
{
    public class BookingService : IBookingServicecs
    {
        private readonly IUnitOfWork _unitOfWork;
        public BookingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //public Task Add(BookingResponseModel booking)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task Delete(object id)
        //{
        //    throw new NotImplementedException();
        //}

        //public async Task<IList<BookingResponseModel>> GetAll()
        //{
        //    // Lấy tất cả người dùng từ repository
        //    var bookings = await _unitOfWork.GetRepository<Bookings>().GetAllAsync();

        //    // Chuyển đổi từ ApplicationUser sang UserResponseModel
        //    var bookingResponseModels = bookings.Select(booking => new BookingResponseModel
        //    {
        //        Description = booking.Description,
        //        Date = booking.Date,
        //        Status = booking.Status,
        //        CustomerId = booking.CustomerId,
        //        EmployeesId = booking.EmployeesId,
        //        OrdersId = booking.OrdersId

        //    }).ToList();

        //    return bookingResponseModels;
        //}

        //public async Task<BookingResponseModel?> GetById(object id)
        //{

        //    var booking = await _unitOfWork.GetRepository<BookingResponseModel>().GetByIdAsync(id);


        //    if (booking == null)
        //        return null;

        //    return new BookingResponseModel
        //    {
        //        Description = booking.Description,
        //        Date = booking.Date,
        //        Status = booking.Status,
        //        CustomerId = booking.CustomerId,
        //        EmployeesId = booking.EmployeesId,
        //        OrdersId = booking.OrdersId
        //    };
        //}

        //public Task Update(BookingResponseModel booking)
        //{
        //    throw new NotImplementedException();
        //}
        public async Task Add(BookingResponseModel bookingVM)
        {
            //booking.Id = Guid.NewGuid().ToString("N");
            IGenericRepository<BookingResponseModel> genericRepository = _unitOfWork.GetRepository<BookingResponseModel>();
            await genericRepository.InsertAsync(bookingVM);
            await _unitOfWork.SaveAsync();
        }

        public async Task Delete(object id)
        {
            IGenericRepository<Bookings> genericRepository = _unitOfWork.GetRepository<Bookings>();
            await genericRepository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }

        public Task<IList<Bookings>> GetAll()
        {
            return _unitOfWork.GetRepository<Bookings>().GetAllAsync();
        }

        public Task<Bookings?> GetById(object id)
        {
            return _unitOfWork.GetRepository<Bookings>().GetByIdAsync(id);
        }

        public async Task Update(Bookings booking)
        {
            IGenericRepository<Bookings> genericRepository = _unitOfWork.GetRepository<Bookings>();
            await genericRepository.UpdateAsync(booking);
            await _unitOfWork.SaveAsync();
        }
    }
}
