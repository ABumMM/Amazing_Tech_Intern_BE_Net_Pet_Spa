using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.Contract.Repositories.Entity
{
    public class BookingPackage
    {
        public string BookingId { get; set; } = string.Empty;
        public virtual Bookings? Booking { get; set; }
        public string PackageId { get; set; } = string.Empty;
        public virtual Packages? Package { get; set; }
        public DateTimeOffset AddedDate { get; set; } // Ngày thêm package vào booking
    }
}
