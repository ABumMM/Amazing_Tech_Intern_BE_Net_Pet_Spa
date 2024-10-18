using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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
using System.Security.Claims;
using System.Threading.Tasks;
using System.Text.Json;
using AutoMapper;
using PetSpa.Core.Infrastructure;

namespace PetSpa.Services.Service
    
{
    public class BookingService : IBookingServicecs
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private string currentUserId => Authentication.GetUserIdFromHttpContextAccessor(_httpContextAccessor);

        public BookingService(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }
        public async Task Add(POSTBookingVM bookingVM)
        {
            // Lấy userId từ token của người dùng đã đăng nhập
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                throw new ErrorException(
                    StatusCodes.Status401Unauthorized,
                    "Unauthorized",
                    "Không tìm thấy thông tin người dùng đăng nhập."
                );
            }

            //mapping
            var booking = _mapper.Map<Bookings>(bookingVM);
            booking.CreatedBy = userId;
            if (bookingVM.Date < DateTimeOffset.Now)
            {
                throw new ErrorException(
                    StatusCodes.Status400BadRequest,
                    "InvalidDate",
                    "Ngày đặt không được nhỏ hơn thời gian hiện tại."
                );
            }
            
            await _unitOfWork.GetRepository<Bookings>().InsertAsync(booking);
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
            var bookingVMs = _mapper.Map<List<GETBookingVM>>(paginatedBookings.Items);
            return new BasePaginatedList<GETBookingVM>(bookingVMs, paginatedBookings.TotalItems, pageNumber, pageSize);
        }

        public async Task<BasePaginatedList<GETBookingVM>> GetAllBookingByCustomer(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Pagenumber and pagesize must greater than 0");
            }



            IQueryable<Bookings> bookings = _unitOfWork.GetRepository<Bookings>()
               .Entities.Where(i => !i.DeletedTime.HasValue && i.CreatedBy == currentUserId)
               .OrderByDescending(c => c.CreatedTime).AsQueryable();
            //Phân trang và chỉ lấy các bản ghi cần thiết
            var paginatedBookings = await bookings
                 .Skip((pageNumber - 1) * pageSize)
                 .Take(pageSize)
                 .ToListAsync();
            return new BasePaginatedList<GETBookingVM>(_mapper.Map<List<GETBookingVM>>(paginatedBookings),
                await bookings.CountAsync(), pageNumber, pageSize);
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
            return _mapper.Map<GETBookingVM>(existedBooking);   
        }
        public async Task Update( POSTBookingVM bookingVM, string id)
        {

            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                throw new ErrorException(
                    StatusCodes.Status401Unauthorized,
                    "Unauthorized",
                    "Không tìm thấy thông tin người dùng đăng nhập."
                );
            }
            var existingBooking = await _unitOfWork.GetRepository<Bookings>().Entities.FirstOrDefaultAsync(p => p.Id == id);
            if (existingBooking == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Booking not found.");
            }
            //kt nếu status đã hủy thì không cho sửa
            if(existingBooking.Status == "Đã hủy")
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Không thể sửa Booking đã bị hủy");
            }    
            var currentTime = DateTimeOffset.Now;
            //kiểm tra nếu trong vòng 24h trước cuộc hẹn ban đầu thì không cho sửa
            if(existingBooking.Date - currentTime < TimeSpan.FromHours(24))
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Không thể dời lịch trong vòng 24 giờ trước cuộc hẹn ban đầu");
            }
            if (existingBooking.Date - currentTime < TimeSpan.FromHours(24))
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Không thể dời lịch trong vòng 24 giờ trước cuộc hẹn ban đầu");
            }
            //không cho cập nhật về ngày dưới ngày hiện tại
            if (bookingVM.Date <= currentTime)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Không thể dời lịch xuống ngày bé hơn ngày hiện tại");
            }
          
            // Ánh xạ dữ liệu cập nhật từ POSTBookingVM sang Bookings
            _mapper.Map(bookingVM, existingBooking);
            existingBooking.LastUpdatedBy = userId;
            existingBooking.LastUpdatedTime = DateTime.Now;
            await _unitOfWork.GetRepository<Bookings>().UpdateAsync(existingBooking);
            await _unitOfWork.SaveAsync();
        }
        public async Task CancelBooking(string bookingId)
        {
            var booking = await _unitOfWork.GetRepository<Bookings>().GetByIdAsync(bookingId);
            if (booking == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, "BookingNotFound", $"Không tìm thấy Booking với ID: {bookingId}");
            }
            if(booking.Status == "Đã hủy")
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Booking này đã bị hủy trước đó");
            }    
            var currentTime = DateTimeOffset.Now;
            if(booking.Date - currentTime < TimeSpan.FromHours(24))
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Không thể hủy trogn 24h trước cuộc hẹn!");
            }

            booking.Status = "Đã hủy"; // 

            // Cập nhật thông tin về người hủy
            booking.LastUpdatedBy = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            booking.LastUpdatedTime = DateTime.Now;
            _unitOfWork.GetRepository<Bookings>().Update(booking);
            await _unitOfWork.SaveAsync();
        }


    }
}
