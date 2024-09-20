
using Microsoft.AspNetCore.Http;
using PetSpa.ModelViews.ModelViews;
using System.ComponentModel.DataAnnotations;

namespace PetSpa.ModelViews.PackageModelViews
{
    public class POSTPackageModelView
    {
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public string? Image { get; set; }
        public string? Information { get; set; }
        public string? Experiences { get; set; }
        public string? CreatedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
    }
}
