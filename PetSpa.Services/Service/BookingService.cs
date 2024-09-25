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
        
        public async Task Add(POSTBookingVM bookingVM)
        {         
            Bookings Booking = new Bookings()
            {
                Description = bookingVM.Description,
                Status = bookingVM.Status,
                Date = bookingVM.Date,
                
                OrdersId = bookingVM.OrdersId,
            };
            await _unitOfWork.GetRepository<Bookings>().InsertAsync(Booking);
            await _unitOfWork.SaveAsync();
        }
        public async Task<BasePaginatedList<GETBookingVM>> GetAll(int pageNumber , int pageSize )
        {
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
            IQueryable<Bookings> query = _unitOfWork.GetRepository<Bookings>().Entities.Where(q => !q.DeletedTime.HasValue);
            var existedBooking = await _unitOfWork.GetRepository<Bookings>().Entities.FirstOrDefaultAsync(p => p.Id == id);   
            if(existedBooking == null)
            {
                return null;
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
        public Task<GETBookingVM?> GetById(object id)
        {
            throw new NotImplementedException();
        }
        public async Task Update( POSTBookingVM bookingVM, string id)
        {
            if (bookingVM == null)
            {
                throw new BadRequestException(ErrorCode.BadRequest, "Booking cannot be null.");
            }
            
            var existingBooking = await _unitOfWork.GetRepository<Bookings>().Entities.FirstOrDefaultAsync(p => p.Id == id);
            if (existingBooking == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Booking not found.");
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
