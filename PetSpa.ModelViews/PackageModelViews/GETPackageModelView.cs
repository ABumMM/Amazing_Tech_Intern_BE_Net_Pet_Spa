using Microsoft.AspNetCore.Http;
using PetSpa.ModelViews.ModelViews;
using PetSpa.ModelViews.PackageServiceModelViews;
using PetSpa.ModelViews.ServiceModelViews;

namespace PetSpa.ModelViews.PackageModelViews
{
    public class GETPackageModelView
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public string? Image { get; set; }
        public string? Information { get; set; }
        public string? Experiences { get; set; }
        public string? CreatedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }

        public List<GETPackageServiceModelView>? listService { get; set; }
       
    }
}
