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
using PetSpa.ModelViews.ServiceModelViews;
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
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "PageNumber và PageSize không hợp lệ!");
            }
            var genericRepository = _unitOfWork.GetRepository<Bookings>();
            IQueryable<Bookings> bookingsQuery = genericRepository.Entities;
            var paginatedBookings = await genericRepository.GetPagging(bookingsQuery, pageNumber, pageSize);

            // Ánh xạ dữ liệu từ Bookings sang GETBookingVM sau khi phân trang
            var bookingVMs = paginatedBookings.Items.Select(b => new GETBookingVM
            {
                Id = b.Id,
                Description = b.Description,
                Date = b.Date,
                Status = b.Status,
                OrdersId = b.OrdersId
            }).ToList();
            return new BasePaginatedList<GETBookingVM>(bookingVMs, paginatedBookings.TotalItems, pageNumber, pageSize);
        }

        public async Task<GETBookingVM?> GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Invalid Booking ID.");
            }
            var existedBooking = await _unitOfWork.GetRepository<Bookings>()
                             .Entities
                             .FirstOrDefaultAsync(p => p.Id == id);
            if (existedBooking == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Not found Booking");
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
            var order = await _unitOfWork.GetRepository<Orders>().GetByIdAsync(bookingVM.OrdersId);

            if (order == null)
            {
                throw new ErrorException(
                StatusCodes.Status404NotFound,
                "OrderNotFound",
                $"Không tìm thấy Order với ID: {bookingVM.OrdersId}"
                );
            }
            existingBooking.Description = bookingVM.Description;
            existingBooking.Status = bookingVM.Status;
            existingBooking.Date = bookingVM.Date;
            existingBooking.OrdersId = bookingVM.OrdersId;
            await _unitOfWork.GetRepository<Bookings>().UpdateAsync(existingBooking);
            await _unitOfWork.SaveAsync();
        }

    }
}
