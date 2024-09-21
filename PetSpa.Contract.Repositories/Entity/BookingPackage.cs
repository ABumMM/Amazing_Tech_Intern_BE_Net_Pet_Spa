using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.Contract.Repositories.Entity
{
    public class BookingPackage
    {
        public string? PackageId { get; set; }
        public virtual Packages? Package { get; set; }
        public string? BookingId { get; set; }
        public virtual Bookings? Booking { get; set; }
        public DateTimeOffset AddedDate { get; set; } // Ngày thêm package vào booking
    }
}
