using PetSpa.Core.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PetSpa.Contract.Repositories.Entity
{
    public class Orders : BaseEntity
    {
        public string? EmployeeID { get; set; }
        public string? OrderID  { get; set; }
        public DateTime Date { get; set; } 

        public string PaymentMethod { get; set; } = string.Empty;

        public double Total { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime LastUpdateTime { get; set; }


        // Khóa ngoại đến Employees
        public virtual ApplicationUser User { get; set; }
        [JsonIgnore]
        public virtual ICollection<Bookings> Bookings { get; set; } = new List<Bookings>();

        // liên kết khóa ngoại khách hàng
        //public virtual ICollection<Bookings> Bookings { get; set; } = new List<Bookings>();

        [JsonIgnore]
        public ICollection<OrdersDetails>? OrdersDetails { get; set;} = new List<OrdersDetails>();


    }
}
