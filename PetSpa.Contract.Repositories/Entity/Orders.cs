using PetSpa.Core.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.Contract.Repositories.Entity
{
    public class Orders : BaseEntity
    {
        public Guid? CustomerID { get; set; }

        public Guid? EmployeeID { get; set; } 

        public DateTime Date { get; set; } 

        public string PaymentMethod { get; set; } = string.Empty;

        public double Total { get; set; }

        // liên kết khóa ngoại nhân viên
        public virtual Employees Employees { get; set; }
        // liên kết khóa ngoại khách hàng
        public virtual Customers Customers { get; set; }
        /*public virtual ICollection<Bookings> Bookings { get; set; } = new List<Bookings>();*/

    }
}
