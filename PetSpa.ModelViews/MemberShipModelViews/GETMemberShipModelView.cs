
namespace PetSpa.ModelViews.MemberShipModelView
{
    public class GETMemberShipModelView
    {
        public string? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int? Point { get; set; }
        public string? SpecialOffer { get; set; }
        public string? CreatedBy { get; set; }
        public DateTimeOffset? CreatedTime { get; set; }
    }
}
