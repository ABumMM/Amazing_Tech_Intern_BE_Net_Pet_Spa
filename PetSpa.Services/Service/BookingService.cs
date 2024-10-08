using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Repositories.IUOW;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.Core.Utils;
using PetSpa.ModelViews.BookingModelViews;
using PetSpa.ModelViews.ModelViews;
using PetSpa.ModelViews.PackageModelViews;
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


        //public async Task<bool> Add(POSTBookingVM bookingVM)
        //{
        //    // Kiểm tra nếu OrdersId là null
        //    if (string.IsNullOrEmpty(bookingVM.OrdersId))
        //    {
        //        return false;
        //    }

        //    // Kiểm tra nếu OrderId không tồn tại
        //    //var existingOrder = await _unitOfWork.GetRepository<Orders>().Entities
        //    //  .FirstOrDefaultAsync(o => o.Id == bookingVM.OrdersId);
        //    var existedBooking = await _unitOfWork.GetRepository<Orders>().Entities.FirstOrDefaultAsync(p => p.Id == bookingVM.OrdersId);

        //    if (existedBooking == null)
        //    {
        //        return false;
        //    }

        //    // Tạo đối tượng Booking mới
        //    Bookings Booking = new Bookings()
        //    {
        //        Description = bookingVM.Description,
        //        Status = bookingVM.Status,
        //        Date = bookingVM.Date,
        //        OrdersId = bookingVM.OrdersId,
        //    };

        //    // Thêm Booking vào cơ sở dữ liệu
        //    await _unitOfWork.GetRepository<Bookings>().InsertAsync(Booking);
        //    await _unitOfWork.SaveAsync();

        //    // Ném ra thông báo thành công
        //    return true;
        //}
        public async Task Add(POSTBookingVM bookingVM)
        {
            
            Bookings Booking = new Bookings()
            {
                Description = bookingVM.Description,
                Status = bookingVM.Status,
                Date = bookingVM.Date,
                OrdersId = bookingVM.OrdersId,
            };
            var order = await _unitOfWork.GetRepository<Orders>().GetByIdAsync(bookingVM.OrdersId);
            
            if (order == null)
            {
                throw new ErrorException(
            StatusCodes.Status404NotFound,
            "OrderNotFound",
            $"Không tìm thấy Order với ID: {bookingVM.OrdersId}"
        );
            }
            await _unitOfWork.GetRepository<Bookings>().InsertAsync(Booking);
            await _unitOfWork.SaveAsync();
        }
        public async Task<BasePaginatedList<GETBookingVM>> GetAll(int pageNumber , int pageSize )
        {
            if (pageSize < 1 || pageNumber <1 )
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Pagenumber and pagesize must greater than 0");
            }
            var bookings = await _unitOfWork.GetRepository<Bookings>().GetAllAsync();

            var bookingViewModels = bookings.Select(bk => new GETBookingVM
            {
                Id = bk.Id,
                Description = bk.Description,
                Date = bk.Date,
                Status = bk.Status,
                OrdersId = bk.OrdersId,

            }).ToList();
            int totalBooking = bookings.Count;

            var paginatedBooking = bookingViewModels
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new BasePaginatedList<GETBookingVM>(paginatedBooking, totalBooking, pageNumber, pageSize);
        }

        public async Task<GETBookingVM?> GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Invalid Booking ID.");
            }
            //var existedBooking = await _unitOfWork.GetRepository<Bookings>().GetByIdAsync(id);
            var existedBooking = await _unitOfWork.GetRepository<Bookings>()
                             .Entities
                             .FirstOrDefaultAsync(p => p.Id == id);
            if (existedBooking == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Not found Package");
            }
            var bookingVM = new GETBookingVM
            {
                    Id = existedBooking.Id,
                    Description = existedBooking.Description,
                    Date = existedBooking.Date,
                    Status = existedBooking.Status,
                    OrdersId = existedBooking.OrdersId,
            };
            return bookingVM;
            
            
        }
        public async Task Update( POSTBookingVM bookingVM, string id)
        {
            var existingBooking = await _unitOfWork.GetRepository<Bookings>().Entities.FirstOrDefaultAsync(p => p.Id == id);
            if (existingBooking == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Booking not found.");
            }
            
            existingBooking.Description = bookingVM.Description;
            existingBooking.Status = bookingVM.Status;
            existingBooking.Date = bookingVM.Date;
            if (bookingVM.OrdersId != null)
            {
                existingBooking.OrdersId = bookingVM.OrdersId;
            }
            await _unitOfWork.GetRepository<Bookings>().UpdateAsync(existingBooking);
            await _unitOfWork.SaveAsync();
        }

    }
}
