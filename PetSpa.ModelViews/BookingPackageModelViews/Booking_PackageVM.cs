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
        public required string BookingId { get; set; }
        public required string PackageId { get; set; }
        //public DateTimeOffset AddedDate { get; set; }
    }
}
