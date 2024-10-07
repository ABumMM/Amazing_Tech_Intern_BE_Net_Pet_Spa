using PetSpa.Contract.Repositories.Entity;
using PetSpa.Core.Base;
using PetSpa.ModelViews.BookingModelViews;
using PetSpa.ModelViews.ModelViews;
using PetSpa.ModelViews.PackageModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.Contract.Services.Interface
{
    public interface IBookingServicecs
    {
        Task<BasePaginatedList<GETBookingVM>> GetAll(int pageNumber, int pageSize);
        Task<GETBookingVM?> GetById(string id);
        //Task<bool> Add(POSTBookingVM bookingVM);
        Task Add(POSTBookingVM bookingVM);
        Task Update( POSTBookingVM bookingVM, string id);
        
    }
}
