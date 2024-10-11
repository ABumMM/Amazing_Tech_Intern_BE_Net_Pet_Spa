

namespace PetSpa.ModelViews.PackageModelViews
{
    public class GETPackageModelView_OrderDetails
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; } = 0;
        public string? Image { get; set; }
        public string? Information { get; set; }
        public string? Experiences { get; set; }
        public string? CreatedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public string? LastUpdatedBy { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
        public string? DeletedBy { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }
    }
}
