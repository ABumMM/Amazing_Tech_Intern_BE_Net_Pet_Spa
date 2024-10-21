using PetSpa.Core.Base;

namespace PetSpa.Contract.Repositories.Entity
{
    public class Bookings : BaseEntity
    {
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public string? Status { get; set; }
        public Guid? ApplicationUserId { get; set; }
        public virtual ApplicationUser? ApplicationUser { get; set; }
        public ICollection<BookingPackage> BookingPackages { get; set; } = new HashSet<BookingPackage>();
    }
}
