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
            //var order = await _unitOfWork.GetRepository<Orders>().GetByIdAsync(bookingVM.OrdersId);

            //if (order == null)
            //{
            //    throw new ErrorException(
            //    StatusCodes.Status404NotFound,
            //    "OrderNotFound",
            //    $"Không tìm thấy Order với ID: {bookingVM.OrdersId}"
            //    );
            //}
            ////Kiểm tra xem ApplicationUserId có tồn tại dưới quyền Employee không
            if (!Guid.TryParse(bookingVM.ApplicationUserId, out var applicationUserId))
            {
                throw new ErrorException(
                    StatusCodes.Status400BadRequest,
                    "InvalidUserId",
                    "ID nhân viên không hợp lệ."
                );
            }

            var applicationUser = await _unitOfWork.GetRepository<ApplicationUser>()
                                 .Entities
                                 .FirstOrDefaultAsync(p => p.Id == applicationUserId);

            if (applicationUser == null)
            {
                throw new ErrorException(
                    StatusCodes.Status404NotFound,
                    "UserNotFound",
                    $"Không tìm thấy nhân viên"
                );
            }
            // Lấy danh sách RoleId của người dùng từ IdentityUserRole
            var userRoles = await _unitOfWork.GetRepository<IdentityUserRole<Guid>>()
                .FindAsync(ur => ur.UserId == applicationUserId);

            if (!userRoles.Any())
            {
                throw new ErrorException(
                    StatusCodes.Status403Forbidden,
                    "NoRolesFound",
                    "Người dùng không có bất kỳ quyền nào."
                );
            }
            var roleIds = userRoles.Select(ur => ur.RoleId).ToList();

            if (!roleIds.Any())
            {
                throw new ErrorException(
                    StatusCodes.Status403Forbidden,
                    "NoRoleIds",
                    "Không tìm thấy RoleId nào liên kết với người dùng."
                );
            }
            //
            var employeeRoleId = await _unitOfWork.GetRepository<ApplicationRole>()
            .Entities
            .Where(role => role.Name == "Employee")
            .Select(role => role.Id)
            .FirstOrDefaultAsync();

            // Nếu không tìm thấy RoleId cho "Employee", ném lỗi
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
            //Phân trang và chỉ lấy các bản ghi cần thiết
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
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Không thể dời lịch, hủy, đổi nhân viên trong vòng 24 giờ trước cuộc hẹn ban đầu");
            }
            //không cho cập nhật về ngày dưới ngày hiện tại
            if (bookingVM.Date <= currentTime)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Không thể dời lịch xuống ngày bé hơn ngày hiện tại");
            }
            //var order = await _unitOfWork.GetRepository<Orders>().GetByIdAsync(bookingVM.OrdersId);

            //if (order == null)
            //{
            //    throw new ErrorException(
            //    StatusCodes.Status404NotFound,
            //    "OrderNotFound",
            //    $"Không tìm thấy Order với ID: {bookingVM.OrdersId}"
            //    );
            //}
            //thêm
            ////Kiểm tra xem ApplicationUserId có tồn tại dưới quyền Employee không
            if (!Guid.TryParse(bookingVM.ApplicationUserId, out var applicationUserId))
            {
                throw new ErrorException(
                    StatusCodes.Status400BadRequest,
                    "InvalidUserId",
                    "ID nhân viên không hợp lệ."
                );
            }

            var applicationUser = await _unitOfWork.GetRepository<ApplicationUser>()
                                 .Entities
                                 .FirstOrDefaultAsync(p => p.Id == applicationUserId);

            if (applicationUser == null)
            {
                throw new ErrorException(
                    StatusCodes.Status404NotFound,
                    "UserNotFound",
                    $"Không tìm thấy nhân viên"
                );
            }
            // Lấy danh sách RoleId của người dùng từ IdentityUserRole
            var userRoles = await _unitOfWork.GetRepository<IdentityUserRole<Guid>>()
                .FindAsync(ur => ur.UserId == applicationUserId);

            if (!userRoles.Any())
            {
                throw new ErrorException(
                    StatusCodes.Status403Forbidden,
                    "NoRolesFound",
                    "Người dùng không có bất kỳ quyền nào."
                );
            }
            var roleIds = userRoles.Select(ur => ur.RoleId).ToList();

            if (!roleIds.Any())
            {
                throw new ErrorException(
                    StatusCodes.Status403Forbidden,
                    "NoRoleIds",
                    "Không tìm thấy RoleId nào liên kết với người dùng."
                );
            }
            //
            var employeeRoleId = await _unitOfWork.GetRepository<ApplicationRole>()
            .Entities
            .Where(role => role.Name == "Employee")
            .Select(role => role.Id)
            .FirstOrDefaultAsync();

            // Nếu không tìm thấy RoleId cho "Employee", ném lỗi
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
                    "Không phải là nhân viên"
                );
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
