namespace PetSpa.ModelViews.ModelViews
{
    public class PackageResponseModel
    {
        public string? Id { get; set; }
        public string? Name{ get; set; }
        public string? Image { get; set; }
        public string? Information { get; set; }
        public string? Experiences { get; set; }
        public List<ServiceEntityResponseModel>? ServiceEntityResponseModels { get; set; } 
    }
}
