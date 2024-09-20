
using Microsoft.AspNetCore.Http;
using PetSpa.ModelViews.ModelViews;
using System.ComponentModel.DataAnnotations;

namespace PetSpa.ModelViews.PackageModelViews
{
    public class POSTPackageViewModel
    {
        public string? Name { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than or equal to 0.")]
        public decimal? Price { get; set; }
        public string? Image { get; set; }
        public string? Information { get; set; }
        public string? Experiences { get; set; }
        public string? CreatedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
    }
}
