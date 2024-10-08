
using Microsoft.AspNetCore.Http;
using PetSpa.ModelViews.ModelViews;
using PetSpa.ModelViews.PackageServiceModelViews;
using System.ComponentModel.DataAnnotations;

namespace PetSpa.ModelViews.PackageModelViews
{
    public class POSTPackageModelView
    {
        public string Name { get; set; }=string.Empty;
        public decimal Price { get; set; } = 0;
        public string? Image { get; set; }
        public string? Information { get; set; }
        public string? Experiences { get; set; }
        public List<string>? ServiceIDs { get; set; }
        public string? CreatedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        
    }
}
