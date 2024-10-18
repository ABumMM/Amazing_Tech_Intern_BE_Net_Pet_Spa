using PetSpa.Core.Base;
namespace PetSpa.Contract.Repositories.Entity
{
    public class Orders : BaseEntity
    {
        //public string? UserId { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public double Total { get; set; } = 0;
        public bool IsPaid { get; set; } = false;
        public Guid CustomerID {  get; set; }
        public virtual ApplicationUser? Customer { get; set; }
        public string? BookingID { get; set; }
        public virtual Bookings? Booking { get; set; }
    }
}