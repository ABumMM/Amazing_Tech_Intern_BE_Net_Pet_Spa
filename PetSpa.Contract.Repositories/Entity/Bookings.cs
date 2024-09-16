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
        public string Status { get; set; }


        // khóa ngoại Customers

        public string? CustomerId { get; set; }
        [JsonIgnore]
        public virtual Customers Customer { get; set; }

        ////khóa ngoại employee
        public string? EmployeesId { get; set; }
        [JsonIgnore]
        public virtual Employees Employee { get; set; }

        ////khóa ngoại oderid

        public string? OrdersId { get; set; }
        [JsonIgnore]
        public virtual Orders Orders { get; set; }

        //1 booking có nhiều gói
        //[JsonIgnore]
        //public virtual ICollection<Packages> Packages { get; set; } = new List<Packages>();
        [JsonIgnore]
        public ICollection<Packages>? Package { get; set; }

    }
}
