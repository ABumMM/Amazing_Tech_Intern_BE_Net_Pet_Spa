using PetSpa.Contract.Repositories.Entity;
using PetSpa.Core.Base;
using PetSpa.ModelViews.BookingModelViews;
using PetSpa.ModelViews.BookingPackageModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.Contract.Services.Interface
{
    public interface IBookingPackage_Service
    {
        Task<BasePaginatedList<GETBooking_PackageVM>> GetAll(int pageNumber, int pageSize);
        Task<List<Booking_PackageVM>> GetById(string id);
        Task Add(Booking_PackageVM bookingPackageVM);
        Task <bool> DeleteBookingPackageAsync(string bookingId, string packageId);
        
    }
}
