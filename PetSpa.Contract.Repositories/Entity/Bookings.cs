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
        [Key]
        public Guid Id { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }

<<<<<<< HEAD
        // khóa ngoại Customers
        public Guid CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customers Customer { get; set; }

        //khóa ngoại employee
        public Guid? EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public virtual Employees Employee { get; set; }

        //khóa ngoại oderid
        public Guid? OrderId { get; set; }
        [ForeignKey("OrderId")]
        public virtual Orders Order { get; set; }

        //khóa ngoại packerid
        public Guid? PackerId { get; set; }
        [ForeignKey("PackageId")]
        public virtual Packages Package { get; set; }
=======
        public Guid CustomerID { get; set; }
        public virtual Customers Customer { get; set; }

        public Guid EmployeeId { get; set; }
        public virtual Employees Employee { get; set; }

        
        public Guid OrderId { get; set; }
        public virtual Orders Order { get; set; }

        public virtual ICollection<Packages> Packages { get; set; }
>>>>>>> 09631208463d3135e650c95629111eb769b49a27
    }
}
