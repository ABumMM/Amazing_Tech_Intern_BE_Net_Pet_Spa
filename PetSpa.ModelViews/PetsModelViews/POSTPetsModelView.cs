
namespace PetSpa.ModelViews.PetsModelViews
{
    public class POSTPetsModelView
    {
        public string? Name { get; set; } 
        public string? Species { get; set; } 
        public decimal Weight { get; set; }
        public string? Breed { get; set; }
        public int Age { get; set; } = 0;
        public string? Description { get; set; }
        public string? Image { get; set; }
        public string? CreatedBy {  get; set; }
        public DateTimeOffset CreatedTime { get; set; }
    }
}
