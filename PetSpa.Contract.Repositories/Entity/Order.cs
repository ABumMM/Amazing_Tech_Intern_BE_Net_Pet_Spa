using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.Contract.Repositories.Entity
{
    internal class Order
    {
        [Key]
        public Guid OrderID { get; set; } 

        public Guid? CustomerID { get; set; }

        public Guid? EmployeeID { get; set; } 

        public DateTime Date { get; set; } 

        public string PaymentMethod { get; set; } = string.Empty;

        public double Total { get; set; } 

        // liên kết khóa ngoại nhân viên
        // liên kết khóa ngoại khách hàng

    }
}
