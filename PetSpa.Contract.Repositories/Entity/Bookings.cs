using PetSpa.Core.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.Contract.Repositories.Entity
{
    public class Bookings : BaseEntity
    {
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }


        // khóa ngoại Customers
       /* public Guid CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customers Customer { get; set; }

        //khóa ngoại employee
        public Guid? EmployeesId { get; set; }
        [ForeignKey("EmployeeId")]
        public virtual Employees Employee { get; set; }*/

        ////khóa ngoại oderid
        ///*public Guid? OrdersId { get; set; }
        //[ForeignKey("OrderId")]
        //public virtual Orders Orders { get; set; }*/

        //khóa ngoại packerid

        /*public Guid? PackerId { get; set; }
        [ForeignKey("PackageId")]
        public virtual Packages Packages { get; set; }

        //public Guid CustomerID { get; set; }
        //public virtual Customers Customers { get; set; }*/
    }
}
