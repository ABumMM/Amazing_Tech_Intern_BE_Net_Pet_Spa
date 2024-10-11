using Microsoft.AspNetCore.Http;
using PetSpa.ModelViews.PackageServiceModelViews;
using PetSpa.ModelViews.ServiceModelViews;
using System.Text.Json.Serialization;

namespace PetSpa.ModelViews.PackageModelViews
{
    public class GETPackageModelView
    {
        public string? Id { get; set; }
        public string Name { get; set; }=string.Empty;
        public decimal? Price { get; set; }
        public string? Image { get; set; }
        public string? Information { get; set; }
        public string? Experiences { get; set; }
        public string? CreatedBy { get; set; }
        public string? LastUpdatedBy { get; set; }
        public string? DeletedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }
    }
}
