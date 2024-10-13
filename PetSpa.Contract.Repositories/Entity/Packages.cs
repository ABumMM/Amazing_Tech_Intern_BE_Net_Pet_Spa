
using PetSpa.Core.Base;
using System.Text.Json.Serialization;

namespace PetSpa.Contract.Repositories.Entity
{
    public class Packages : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; } = 0;
        public string? Image { get; set; }
        public string? Information { get; set; }
        public string? Experiences { get; set; }
        [JsonIgnore]
        public ICollection<PackageServiceEntity>? PackageServices { get; set; }

        [JsonIgnore]
        public ICollection<BookingPackage>? BookingPackages { get; set; } = new HashSet<BookingPackage>();

        [JsonIgnore]
        public ICollection<OrdersDetails>? OrdersDetails { get; set; } = new HashSet<OrdersDetails>();

        [JsonIgnore]
        public ICollection<Reviews>? Reviews { get; set; } = new HashSet<Reviews>();


    }
}
