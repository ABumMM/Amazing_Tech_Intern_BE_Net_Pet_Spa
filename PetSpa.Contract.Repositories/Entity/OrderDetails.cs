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
    public class OrdersDetails : BaseEntity 
    {
        public int Quantity { get; set; }
        public string Status { get; set; }
        public double Price { get; set; } = 0;
        public Guid? OrderId { get; set; }
        public Guid? Packed { get; set; }
    }
}
