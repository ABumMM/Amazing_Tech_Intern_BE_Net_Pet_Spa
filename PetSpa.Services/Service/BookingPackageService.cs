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
using AutoMapper;
namespace PetSpa.Services.Service
{
    public class BookingPackageService : IBookingPackage_Service
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookingPackageService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task Add(Booking_PackageVM bookingPackageVM)
        {
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
            var booKing = await _unitOfWork.GetRepository<Bookings>().GetByIdAsync(bookingPackageVM.BookingId);
            if (booKing == null)
            {
                throw new ErrorException(
                    StatusCodes.Status404NotFound,
                    "BookingNotFound",
                    $"Không tìm thấy Booking với ID: {bookingPackageVM.BookingId}"
                );
            }
            var pacKage = await _unitOfWork.GetRepository<Packages>().GetByIdAsync(bookingPackageVM.PackageId);
            if (pacKage == null)
            {
                throw new ErrorException(
                    StatusCodes.Status404NotFound,
                    "PackageNotFound",
                    $"Không tìm thấy Package với ID: {bookingPackageVM.PackageId}"
                );
            }

            // Sử dụng AutoMapper để ánh xạ từ Booking_PackageVM sang BookingPackage
            var bookingPackage = _mapper.Map<BookingPackage>(bookingPackageVM);
            bookingPackage.AddedDate = DateTime.Now;

            await _unitOfWork.GetRepository<BookingPackage>().InsertAsync(bookingPackage);
            await _unitOfWork.SaveAsync();
        }
        public async Task<BasePaginatedList<GETBooking_PackageVM>> GetAll(int pageNumber, int pageSize)
        {
            if (pageSize < 1 || pageNumber < 1)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "PageNumber và PageSize không hợp lệ!");
            }

            var bookingRepository = _unitOfWork.GetRepository<Bookings>();
            var bookingPackageRepository = _unitOfWork.GetRepository<BookingPackage>();

            var bookingsWithPackagesQuery = bookingRepository.Entities
                .Join(
                    bookingPackageRepository.Entities.Include(bp => bp.Package),
                    b => b.Id,
                    bp => bp.BookingId,
                    (b, bp) => new { Booking = b, BookingPackage = bp }
                )
                .Where(x => x.BookingPackage.Package != null)
                .GroupBy(x => x.Booking)
                .Select(g => new
                {
                    Booking = g.Key,
                    Packages = g.Select(x => new PackageDTO
                    {
                        Id = x.BookingPackage.PackageId,
                        Name = x.BookingPackage.Package != null ? x.BookingPackage.Package.Name : "Unknown",
                        Price = x.BookingPackage.Package != null ? x.BookingPackage.Package.Price : 0
                    }).ToList()
                });

            int totalBookings = await bookingsWithPackagesQuery.CountAsync();

            var paginatedBookings = await bookingsWithPackagesQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var bookingWithPackagesVMs = paginatedBookings.Select(b =>
            {
                var bookingVm = _mapper.Map<GETBooking_PackageVM>(b.Booking);
                bookingVm.Packages = b.Packages; 
                return bookingVm;
            }).ToList();

            return new BasePaginatedList<GETBooking_PackageVM>(bookingWithPackagesVMs, totalBookings, pageNumber, pageSize);
        }

        public async Task<GETBooking_PackageVM> GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Id không được để trống");
            }
            var existedBookingPKs = await _unitOfWork.GetRepository<BookingPackage>()
                .Entities
                .Where(p => p.BookingId == id)
                .ToListAsync();

            if (!existedBookingPKs.Any())
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Không tìm thấy ");
            }
            var booking = await _unitOfWork.GetRepository<Bookings>()
                .Entities
                .FirstOrDefaultAsync(b => b.Id == id);

            // Lấy danh sách booking packages và bao gồm thông tin package
            var bookingPackages = await _unitOfWork.GetRepository<BookingPackage>()
                .Entities
                .Where(bp => bp.BookingId == id)
                .Include(bp => bp.Package)
                .ToListAsync();
            var packages = _mapper.Map<List<PackageDTO>>(bookingPackages);
            var bookingWithPackagesVM = _mapper.Map<GETBooking_PackageVM>(booking);
            bookingWithPackagesVM.Packages = packages;

            return bookingWithPackagesVM;
        }

    }
}
