

namespace PetSpa.ModelViews.ReviewModelViews
{
    public class GETReviewModelViews
    {
        public required string Id {  get; set; }
        public string Description { get; set; } = string.Empty;
        public required string PackageID { get; set; }
        public string? CreatedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
    }
}
