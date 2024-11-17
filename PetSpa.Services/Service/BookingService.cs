using Microsoft.AspNetCore.Http;using Microsoft.EntityFrameworkCore;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Repositories.IUOW;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.ModelViews.BookingModelViews;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PetSpa.Repositories.UOW;
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
            if (string.IsNullOrEmpty(currentUserId))
            {
                throw new ErrorException(
                    StatusCodes.Status401Unauthorized,
                    "Unauthorized",
                    "Không tìm thấy thông tin người dùng đăng nhập."
                );
            }

            //mapping
            var booking = _mapper.Map<Bookings>(bookingVM);
            booking.CreatedBy = currentUserId;
            if (bookingVM.Date < DateTimeOffset.Now)
            {
                throw new ErrorException(
                    StatusCodes.Status400BadRequest,
                    "InvalidDate",
                    "Ngày đặt không được nhỏ hơn thời gian hiện tại."
                );
            }
            if (!Guid.TryParse(bookingVM.ApplicationUserId, out var applicationUserId))
            {
                throw new ErrorException(
                    StatusCodes.Status400BadRequest,
                    "InvalidUserId",
                    "ID nhân viên không hợp lệ."
                );
            }
            var userExists = await _unitOfWork.GetRepository<ApplicationUser>()
                                    .Entities
                                    .AnyAsync(p => p.Id == applicationUserId);

            if (!userExists)
            {
                throw new ErrorException(
                    StatusCodes.Status404NotFound,
                    "UserNotFound",
                    "Không tìm thấy nhân viên"
                );
            }
            var employeeRoleId = await _unitOfWork.GetRepository<ApplicationRole>()
                .Entities
                .Where(role => role.Name == "Employee")
                .Select(role => role.Id)
                .FirstOrDefaultAsync();
            if (employeeRoleId == default)
            {
                throw new ErrorException(
                    StatusCodes.Status403Forbidden,
                    "RoleNotFound",
                    "Không tìm thấy quyền Employee."
                );
            }
            var isEmployee = await _unitOfWork.GetRepository<IdentityUserRole<Guid>>()
                .Entities
                .AnyAsync(ur => ur.UserId == applicationUserId && ur.RoleId == employeeRoleId);

            if (!isEmployee)
            {
                throw new ErrorException(
                    StatusCodes.Status403Forbidden,
                    "NotAnEmployee",
                    "Người dùng không có quyền Employee."
                );
            }


            booking.ApplicationUserId = Guid.Parse(bookingVM.ApplicationUserId);
             await _unitOfWork.GetRepository<Bookings>().InsertAsync(booking);
             await _unitOfWork.SaveAsync();
        }
        public async Task<BasePaginatedList<GETBookingVM>> GetAll(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Pagenumber and pagesize must greater than 0");
            }
            IQueryable<Bookings> bookings = _unitOfWork.GetRepository<Bookings>()
               .Entities.Where(i => !i.DeletedTime.HasValue)
               .OrderByDescending(c => c.CreatedTime).AsQueryable();
            var paginatedBookings = await bookings
                 .Skip((pageNumber - 1) * pageSize)
                 .Take(pageSize)
                 .ToListAsync();
            return new BasePaginatedList<GETBookingVM>(_mapper.Map<List<GETBookingVM>>(paginatedBookings),
                await bookings.CountAsync(), pageNumber, pageSize);
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
            if (string.IsNullOrEmpty(currentUserId))
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
            if(existingBooking.Status == "Đã hủy")
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Không thể sửa Booking đã bị hủy");
            }    
            var currentTime = DateTimeOffset.Now;
            if(existingBooking.Date - currentTime < TimeSpan.FromHours(24))
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Không thể dời lịch, hủy, đổi nhân viên trong vòng 24 giờ trước cuộc hẹn ban đầu");
            }
            //không cho cập nhật về ngày dưới ngày hiện tại
            if (bookingVM.Date <= currentTime)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Không thể dời lịch xuống ngày bé hơn ngày hiện tại");
            }
            if (!Guid.TryParse(bookingVM.ApplicationUserId, out var applicationUserId))
            {
                throw new ErrorException(
                    StatusCodes.Status400BadRequest,
                    "InvalidUserId",
                    "ID nhân viên không hợp lệ."
                );
            }
            var userExists = await _unitOfWork.GetRepository<ApplicationUser>()
                                    .Entities
                                    .AnyAsync(p => p.Id == applicationUserId);

            if (!userExists)
            {
                throw new ErrorException(
                    StatusCodes.Status404NotFound,
                    "UserNotFound",
                    "Không tìm thấy nhân viên"
                );
            }
            var employeeRoleId = await _unitOfWork.GetRepository<ApplicationRole>()
                .Entities
                .Where(role => role.Name == "Employee")
                .Select(role => role.Id)
                .FirstOrDefaultAsync();
            if (employeeRoleId == default)
            {
                throw new ErrorException(
                    StatusCodes.Status403Forbidden,
                    "RoleNotFound",
                    "Không tìm thấy quyền Employee."
                );
            }
            var isEmployee = await _unitOfWork.GetRepository<IdentityUserRole<Guid>>()
                .Entities
                .AnyAsync(ur => ur.UserId == applicationUserId && ur.RoleId == employeeRoleId);

            if (!isEmployee)
            {
                throw new ErrorException(
                    StatusCodes.Status403Forbidden,
                    "NotAnEmployee",
                    "Người dùng không có quyền Employee."
                );
            }
            _mapper.Map(bookingVM, existingBooking);
            existingBooking.LastUpdatedBy = currentUserId;
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

            booking.Status = "Đã hủy"; 
            booking.LastUpdatedBy = currentUserId;
            booking.LastUpdatedTime = DateTime.Now;
            _unitOfWork.GetRepository<Bookings>().Update(booking);
            await _unitOfWork.SaveAsync();
        }
    }
}
