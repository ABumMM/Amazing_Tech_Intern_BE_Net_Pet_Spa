using PetSpa.Core.Base;
namespace PetSpa.Contract.Repositories.Entity
{
    public class OrdersDetails : BaseEntity
    {
        public int Quantity { get; set; } = 0;
        public string? Status { get; set; }
        public decimal Price { get; set; } = 0;
        public required string OrderID { get; set; }
        public string? PackageID { get; set; }
        //Mối kết hợp 1-n
        public virtual Orders? Order { get; set; }
        public virtual Packages? Package { get; set; }
    }
}