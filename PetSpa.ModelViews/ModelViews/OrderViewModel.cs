using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.ModelViews.ModelViews
{
    public class OrderViewModel
    {
        public string Id { get; set; }
        public string CustomerName { get; set; } // Hiển thị tên khách hàng
        public string EmployeeName { get; set; } // Hiển thị tên nhân viên
        public DateTime Date { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public double Total { get; set; }
    }
}
