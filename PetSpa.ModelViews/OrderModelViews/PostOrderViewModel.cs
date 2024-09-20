using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.ModelViews.OrderModelViews
{
    public class PostOrderViewModel
    {
        public string OrderID { get; set; } = string.Empty;
        public string CustomerID { get; set; } = string.Empty;
        public string EmployeeID { get; set; } = string.Empty;
        public DateTime? Date { get; set; }
        public string PaymentMethod { get; set; } = "Unknown";
        public decimal Total { get; set; }
    }
}
