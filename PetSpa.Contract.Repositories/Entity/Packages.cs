
using PetSpa.Core.Base;
using System.Text.Json.Serialization;

namespace PetSpa.Contract.Repositories.Entity
{
    public class Packages : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public decimal? Price { get; set; }
        public string? Image { get; set; }
        public string? Information { get; set; }
        public string? Experiences { get; set; }
        public string? OrderDetailID { get; set; }


        [JsonIgnore]
        public ICollection<Services>? Services { get; set; }

        [JsonIgnore]
        public ICollection<PackageServiceEntity>? PackageServices { get; set; }

        [JsonIgnore]
        public ICollection<BookingPackage>? BookingPackages { get; set; } = new HashSet<BookingPackage>();

        public virtual OrdersDetails? OrdersDetails { get; set; }

        //Mối quan hệ 1-n
        [JsonIgnore]
        public virtual ICollection<OrderDetailPackage> OrderDetailPackages { get; set; } = new List<OrderDetailPackage>();

    }
}
