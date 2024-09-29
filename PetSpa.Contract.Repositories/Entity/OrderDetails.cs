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
        public decimal? Price { get; set; }
        public string? OrderID { get; set; }
        public string? PackageID { get; set; }

        //Mối kết hợp 1-n
        public virtual Orders? Orders { get; set; }

        public ICollection<Packages> Packages { get; set; } = new List<Packages>();


        //Mối Kết Hợp Nhiều Nhiều
        public virtual ICollection<OrderDetailPackage> OrderDetailPackages { get; set; } = new List<OrderDetailPackage>();
    }
}