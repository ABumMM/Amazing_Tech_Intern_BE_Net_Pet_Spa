using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.ModelViews.PackageModelViews
{
    public class PUTPackageModelView
    {
        public required string Id { get; set; }
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public string? Image { get; set; }
        public string? Information { get; set; }
        public string? Experiences { get; set; }
        public string? LastUpdatedBy { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
    }
}
