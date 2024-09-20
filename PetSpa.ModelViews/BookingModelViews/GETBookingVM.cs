using PetSpa.ModelViews.ModelViews;
using PetSpa.ModelViews.PackageModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.ModelViews.BookingModelViews
{
    public class GETBookingVM
    {
        public string Id { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public string? Status { get; set; }

        public string? OrdersId { get; set; }
        public List<PackageResponseModel>? packageResponseModels { get; set; }
        //public List<GETPackageViewModel>? getPackageViewModel { get; set; }
    }
}
