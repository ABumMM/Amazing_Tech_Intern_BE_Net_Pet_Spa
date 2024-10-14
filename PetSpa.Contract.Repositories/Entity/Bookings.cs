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
       // public required string OrdersId { get; set; }     
       // public virtual Orders? Orders { get; set; }
        //thêm
        public Guid? ApplicationUserId { get; set; }
        public virtual ApplicationUser? ApplicationUser { get; set; }
        //đến đây
        public ICollection<BookingPackage> BookingPackages { get; set; } = new HashSet<BookingPackage>();
    }
}
