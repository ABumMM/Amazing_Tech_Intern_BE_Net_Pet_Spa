using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.ModelViews.OrderModelViews
{
    public class PutOrderViewModel
    {
        public string Id { get; set; }
        public string? EmployeeID { get; set; }
        public DateTime? Date { get; set; }
        public string? PaymentMethod { get; set; }
        public double? Total { get; set; }
        public string? LastUpdatedBy { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
    }
}
