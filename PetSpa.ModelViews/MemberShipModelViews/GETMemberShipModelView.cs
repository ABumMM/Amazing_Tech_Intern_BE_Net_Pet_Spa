
namespace PetSpa.ModelViews.MemberShipModelView
{
    public class GETMemberShipModelView
    {
        public string? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double TotalSpent { get; set; } = 0;//Tổng số tiền đã cho tiêu
        public double DiscountRate { get; set; } = 0;
        public string? CreatedBy { get; set; }
        public DateTimeOffset? CreatedTime { get; set; }
    }
}
