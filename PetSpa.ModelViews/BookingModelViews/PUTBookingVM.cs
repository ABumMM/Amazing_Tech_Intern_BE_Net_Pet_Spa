using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.ModelViews.BookingModelViews
{
    public class PUTBookingVM
    {
        public string Id { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? Status { get; set; }
        public DateTime Date { get; set; }
        public string? CustomerId { get; set; }
        public string? EmployeesId { get; set; }

        public string? OrdersId { get; set; }
    }
}
