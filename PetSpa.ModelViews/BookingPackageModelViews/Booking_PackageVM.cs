using PetSpa.Contract.Repositories.Entity;
using PetSpa.ModelViews.PackageModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.ModelViews.BookingPackageModelViews
{
    public class Booking_PackageVM
    {
        public string? BookingId { get; set; }
        public string? PackageId { get; set; }
        public DateTimeOffset AddedDate { get; set; }
        //public string? Description { get; set; }
        //public DateTime Date { get; set; }
        //public string? Status { get; set; }

        //public List<GETPackageModelView> Packages { get; set; } = new List<GETPackageModelView>();
    }
}
