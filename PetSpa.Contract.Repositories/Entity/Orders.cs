﻿using PetSpa.Core.Base;
namespace PetSpa.Contract.Repositories.Entity
{
    public class Orders : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public decimal Total { get; set; } = 0;
        public string Status { get; set; } = "Pending"; // Pending/Success/Failed
        public decimal DiscountPrice { get; set; } = 0;

        public decimal FinalPrice { get; set; } = 0; // số tiền phải trả
        public bool IsPaid { get; set; } = false;
        public Guid CustomerID { get; set; }
        public virtual ApplicationUser? Customer { get; set; }

        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

        public virtual ICollection<OrdersDetails> OrdersDetails { get; set; } = new List<OrdersDetails>();

        public virtual ICollection<Bookings> Bookings { get; set; } = new List<Bookings>();
    }
}