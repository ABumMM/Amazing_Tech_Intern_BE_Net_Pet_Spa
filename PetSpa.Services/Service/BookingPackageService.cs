using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Repositories.IUOW;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.ModelViews.BookingModelViews;
using PetSpa.ModelViews.BookingPackageModelViews;
using PetSpa.ModelViews.PackageModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.Services.Service
{
    public class BookingPackageService : IBookingPackage_Service
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public BookingPackageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task Add(Booking_PackageVM bookingPackageVM)
        {
            // Kiểm tra trùng lặp với BookingId và PackageId
            var existingBookingPackage = await _unitOfWork.GetRepository<BookingPackage>().Entities
                .FirstOrDefaultAsync(bp => bp.BookingId == bookingPackageVM.BookingId &&
                                           bp.PackageId == bookingPackageVM.PackageId);
            if (existingBookingPackage != null)
            {
                throw new ErrorException(
                    statusCode: StatusCodes.Status400BadRequest,
                    errorCode: ErrorCode.BadRequest,
                    message: "BookingPackage với BookingID và PackageID này đã tồn tại.");
            }
            //kt booking co hay khong
            var booKing = await _unitOfWork.GetRepository<Bookings>().GetByIdAsync(bookingPackageVM.BookingId);

            if (booKing == null)
            {
                throw new ErrorException(
                StatusCodes.Status404NotFound,
                "BookingNotFound",
                $"Không tìm thấy Booking với ID: {bookingPackageVM.BookingId}"
                );
            }
            //kt package co hay khong
            var pacKage = await _unitOfWork.GetRepository<Packages>().GetByIdAsync(bookingPackageVM.PackageId);

            if (pacKage == null)
            {
                throw new ErrorException(
                StatusCodes.Status404NotFound,
                "PackageNotFound",
                $"Không tìm thấy Package với ID: {bookingPackageVM.PackageId}"
                );
            }
            BookingPackage bookingPackage = new BookingPackage()
            {
                PackageId = bookingPackageVM.PackageId,
                BookingId = bookingPackageVM.BookingId,
                AddedDate = bookingPackageVM.AddedDate,
            };

            await _unitOfWork.GetRepository<BookingPackage>().InsertAsync(bookingPackage);
            await _unitOfWork.SaveAsync();
        }
        public async Task<BasePaginatedList<GETBooking_PackageVM>> GetAll(int pageNumber, int pageSize)
        {
            if (pageSize < 1 || pageNumber < 1)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "PageNumber và PageSize không hợp lệ!");
            }

            // Truy vấn các Booking có liên kết với BookingPackage và phân trang trực tiếp
            var bookingRepository = _unitOfWork.GetRepository<Bookings>();
            var bookingPackageRepository = _unitOfWork.GetRepository<BookingPackage>();
            var bookingsWithPackagesQuery = bookingRepository.Entities
                .Join(
                    bookingPackageRepository.Entities, // Liên kết với bảng BookingPackage
                    b => b.Id,                         // Khóa chính của Bookings
                    bp => bp.BookingId,                // Khóa ngoại của BookingPackage
                    (b, bp) => new { Booking = b, Package = bp.Package } // Chọn các thông tin cần thiết
                )
                .Where(x => x.Package != null)         // Lọc ra chỉ những Booking có Package
                .GroupBy(x => x.Booking)               // Nhóm theo Booking
                .Select(g => new
                {
                    Booking = g.Key,
                    Packages = g.Select(x => new PackageDTO
                    {
                        Id = x.Package!.Id, //! để loại bỏ cảnh báo null vì trước đó đã kiểm tra Package != null trong where
                        Name = x.Package.Name,
                        Price = x.Package.Price.GetValueOrDefault()
                    }).ToList()
                });
            int totalBookings = await bookingsWithPackagesQuery.CountAsync();
            var paginatedBookings = await bookingsWithPackagesQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var bookingWithPackagesVMs = paginatedBookings.Select(b => new GETBooking_PackageVM
            {
                BookingId = b.Booking.Id,
                Description = b.Booking.Description,
                Date = b.Booking.Date,
                Status = b.Booking.Status,
                Packages = b.Packages
            }).ToList();
            return new BasePaginatedList<GETBooking_PackageVM>(bookingWithPackagesVMs, totalBookings, pageNumber, pageSize);
        }
        public async Task<GETBooking_PackageVM> GetById(string id)
        {
            if (id == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Id không được để trống");

            }
            var existedBookingPKs = await _unitOfWork.GetRepository<BookingPackage>()
                .Entities
                .Where(p => p.BookingId == id)
                .ToListAsync();
            if (!existedBookingPKs.Any())
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Không tìm thấy ");

            }
            // Duyệt qua tất cả các bản ghi và chuyển đổi thành ViewModel
            var bookingPKVMs = existedBookingPKs.Select(existedBookingPK => new Booking_PackageVM
            {
                BookingId = existedBookingPK.BookingId,
                PackageId = existedBookingPK.PackageId,
                AddedDate = existedBookingPK.AddedDate,
            }).ToList();
            var booking = await _unitOfWork.GetRepository<Bookings>()
            .Entities
            .FirstOrDefaultAsync(b => b.Id == id);
            if (booking == null)
            {
                return null; 
            }
            var bookingPackages = await _unitOfWork.GetRepository<BookingPackage>()
                .Entities
                .Where(bp => bp.BookingId == id)
                .Include(bp => bp.Package)
                .ToListAsync();
            var packages = bookingPackages.Select(bp => new PackageDTO
            {
                Id = bp.PackageId,
                Name = bp.Package?.Name,
                Price = bp.Package?.Price.GetValueOrDefault()
            }).ToList();
            var bookingWithPackagesVM = new GETBooking_PackageVM
            {
                BookingId = booking.Id,
                Description = booking.Description,
                Date = booking.Date,
                Status = booking.Status,
                Packages = packages
            };

            return bookingWithPackagesVM;
        }

    }
}
