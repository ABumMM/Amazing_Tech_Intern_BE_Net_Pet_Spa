using Microsoft.AspNetCore.Http;
using PetSpa.ModelViews.ModelViews;

namespace PetSpa.ModelViews.PackageModelViews
{
    public class GETPackageViewModel
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public string? Image { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public string? Information { get; set; }
        public string? Experiences { get; set; }
        public string? CreatedBy { get; set; }

        public List<ServiceEntityResponseModel>? ServiceEntityResponseModels { get; set; }
    }
}
