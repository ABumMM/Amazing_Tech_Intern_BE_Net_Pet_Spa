﻿using PetSpa.Core.Base;
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
        //public string? UserId { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public double Total { get; set; }
        public bool IsPaid { get; set; } = false;
        //public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Bookings> Bookings { get; set; } = new List<Bookings>();
        public virtual ICollection<OrdersDetails> OrderDetails { get; set; } = new List<OrdersDetails>();
    }
}