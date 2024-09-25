
using PetSpa.Core.Base;
using System.Text.Json.Serialization;

namespace PetSpa.Contract.Repositories.Entity
{
    public class Packages: BaseEntity
    {
        public string Name { get; set; }=string.Empty;
        public decimal? Price { get; set; }
        public string? Image {  get; set; }
        public string? Information {  get; set; }
        public string? Experiences {  get; set; }
        [JsonIgnore]
        public ICollection<PackageServiceDTO>? PackageServices { get; set; }

        [JsonIgnore]
        public ICollection<BookingPackage>? BookingPackages { get; set; } = new HashSet<BookingPackage>();

    }
}
