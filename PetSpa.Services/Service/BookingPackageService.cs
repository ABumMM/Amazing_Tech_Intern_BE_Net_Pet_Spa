﻿using Microsoft.AspNetCore.Http;
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
            BookingPackage BookingPackage = new BookingPackage()
            {
                PackageId = bookingPackageVM.PackageId,
                BookingId = bookingPackageVM.BookingId,
                AddedDate = bookingPackageVM.AddedDate,
            };
            await _unitOfWork.GetRepository<BookingPackage>().InsertAsync(BookingPackage);
            await _unitOfWork.SaveAsync();
        }
        public async Task<BasePaginatedList<GETBooking_PackageVM>> GetAll(int pageNumber, int pageSize)
        {
            // Truy vấn tất cả Bookings
            var bookings = await _unitOfWork.GetRepository<Bookings>().Entities
                .ToListAsync();

            if (bookings == null || !bookings.Any())
            {
                return new BasePaginatedList<GETBooking_PackageVM>(new List<GETBooking_PackageVM>(), 0, pageNumber, pageSize);
            }
            var bookingPackages = await _unitOfWork.GetRepository<BookingPackage>().Entities
                .Include(bp => bp.Package)
                .ToListAsync();
            var packageGroups = bookingPackages.GroupBy(bp => bp.BookingId)
                .ToDictionary(group => group.Key, group => group.Select(bp => new GETPackageModelView
                {
                    Id = bp.PackageId,
                    Name = bp.Package.Name,
                    Price = bp.Package.Price.GetValueOrDefault(),
                }).ToList());

            // Ánh xạ Booking vào ViewModel
            var bookingWithPackagesVMs = bookings.Select(b => new GETBooking_PackageVM
            {
                BookingId = b.Id,
                Description = b.Description,
                Date = b.Date,
                Status = b.Status,
                Packages = packageGroups.ContainsKey(b.Id) ? packageGroups[b.Id] : new List<GETPackageModelView>()
            }).ToList();

            // Phân trang
            int totalBookings = bookingWithPackagesVMs.Count;
            var paginatedBookingPK = bookingWithPackagesVMs
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new BasePaginatedList<GETBooking_PackageVM>(paginatedBookingPK, totalBookings, pageNumber, pageSize);
        }


        //public async Task<BasePaginatedList<Booking_PackageVM>> GetAll(int pageNumber, int pageSize)
        //{
        //    var bookingPackages = await _unitOfWork.GetRepository<BookingPackage>().GetAllAsync();

        //    var bookingPKViewModels = bookingPackages.Select(bk => new Booking_PackageVM
        //    {
        //        BookingId = bk.BookingId,
        //        PackageId = bk.PackageId,
        //        AddedDate = bk.AddedDate,

        //    }).ToList();
        //    int totalBookingPK = bookingPackages.Count;

        //    var paginatedBookingPK = bookingPKViewModels
        //        .Skip((pageNumber - 1) * pageSize)
        //        .Take(pageSize)
        //        .ToList();

        //    return new BasePaginatedList<Booking_PackageVM>(paginatedBookingPK, totalBookingPK, pageNumber, pageSize);
        //}
        public async Task<List<Booking_PackageVM>> GetById(string id)
        {
            if (id != null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Point must be greater than or equal to 0.");

            }
            var existedBookingPKs = await _unitOfWork.GetRepository<BookingPackage>()
                .Entities
                .Where(p => p.BookingId == id)
                .ToListAsync();
            if (!existedBookingPKs.Any())
            {
                return null;
            }
            // Duyệt qua tất cả các bản ghi và chuyển đổi thành ViewModel
            var bookingPKVMs = existedBookingPKs.Select(existedBookingPK => new Booking_PackageVM
            {
                BookingId = existedBookingPK.BookingId,
                PackageId = existedBookingPK.PackageId,
                AddedDate = existedBookingPK.AddedDate,
            }).ToList();

            return bookingPKVMs;
        }
        public async Task<bool> DeleteBookingPackageAsync(string bookingId, string packageId)
        {
            var repository = _unitOfWork.GetRepository<BookingPackage>();
            //bookingId = bookingId.Trim();
            //packageId = packageId.Trim();
            var bookingPackage = await repository.GetByKeysAsync(bookingId, packageId);

            if (bookingPackage == null)
            {
                return false; 
            }
            repository.Delete1(bookingPackage);
            await _unitOfWork.SaveAsync();

            return true; 
        }
    }
}
