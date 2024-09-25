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



        public DateTime Date { get; set; } 

        public string PaymentMethod { get; set; } = string.Empty;

        public double Total { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime LastUpdateTime { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<Bookings> Bookings { get; set; } = new List<Bookings>();

        // liên kết khóa ngoại khách hàng
        //public virtual ICollection<Bookings> Bookings { get; set; } = new List<Bookings>();


    }
}
