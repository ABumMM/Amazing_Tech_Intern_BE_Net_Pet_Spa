using PetSpa.Core.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public Guid CustomerID { get; set; }
        public virtual Customers Customer { get; set; }

        public Guid EmployeeId { get; set; }
        public virtual Employees Employee { get; set; }

        
        public Guid OrderId { get; set; }
        public virtual Orders Order { get; set; }

        public virtual ICollection<Packages> Packages { get; set; }
    }
}
