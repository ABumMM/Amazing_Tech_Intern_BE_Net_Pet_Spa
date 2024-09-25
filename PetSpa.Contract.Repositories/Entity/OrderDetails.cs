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
    public class OrdersDetails : BaseEntity 
    {
        public int Quantity { get; set; }
        public string? Status { get; set; }
        public decimal Price { get; set; } = 0;
        public string? OrderId { get; set; }
        public string? PackageId { get; set; }

        [JsonIgnore]
        public ICollection<Orders>? Orders { get; set; } = new HashSet<Orders>();

        //[JsonIgnore]
        //public ICollection<Packages>? Packages { get; set; } = new HashSet<Packages>();
    }
}
