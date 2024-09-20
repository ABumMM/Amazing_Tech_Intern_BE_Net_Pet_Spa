using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.ModelViews.OrderModelViews
{
    public class GetOrderViewModel
    {
        public string? Id { get; set; }
        public string? CustomerID { get; set; }
        public string? EmployeeID { get; set; }
        public DateTime? Date { get; set; }
        public string? PaymentMethod { get; set; }
        public decimal? Total { get; set; }
    }
}
