using PetSpa.Core.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PetSpa.Contract.Repositories.Entity
{
    public class Bookings : BaseEntity
    {
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public string? Status { get; set; }

       
        public string CustomerId { get; set; } =string.Empty;
        [JsonIgnore]
        public virtual Customers? Customer { get; set; }

     
        public string EmployeesId { get; set; } = string.Empty;
        [JsonIgnore]
        public virtual Employees? Employee { get; set; }

        public string OrdersId { get; set; } = string.Empty;
        [JsonIgnore]
        public virtual Orders? Orders { get; set; }

        
        [JsonIgnore]
        public ICollection<Packages>? Package { get; set; } = new List<Packages>();

    }
}
