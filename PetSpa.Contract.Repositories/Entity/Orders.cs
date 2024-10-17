﻿using PetSpa.Core.Base;
namespace PetSpa.Contract.Repositories.Entity
{
    public class Orders : BaseEntity
    {
        public string PaymentMethod { get; set; } = string.Empty;
        public decimal Total { get; set; } = 0;
        public bool IsPaid { get; set; } = false;
        public Guid CustomerID {  get; set; }
        public virtual ApplicationUser? Customer { get; set; }
        public virtual ICollection<Bookings> Bookings { get; set; } = new List<Bookings>();
    }
}