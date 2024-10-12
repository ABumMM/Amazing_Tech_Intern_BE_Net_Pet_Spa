namespace PetSpa.ModelViews.PackageModelViews
{
    public class POSTPackageModelView
    {
        public string Name { get; set; }=string.Empty;
        public decimal Price { get; set; } = 0;
        public string? Image { get; set; }
        public string? Information { get; set; }
        public string? Experiences { get; set; }
    }
}
