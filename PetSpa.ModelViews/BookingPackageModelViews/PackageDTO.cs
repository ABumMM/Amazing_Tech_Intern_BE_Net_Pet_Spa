using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.ModelViews.BookingPackageModelViews
{
    public class PackageDTO
    {
        public string Id { get; set; } = string.Empty;
        public string? Name { get; set; }
        public decimal? Price { get; set; }
    }
}
