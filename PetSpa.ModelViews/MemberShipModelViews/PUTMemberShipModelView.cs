using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.ModelViews.MemberShipModelViews
{
    public class PUTMemberShipModelView
    {
        public required string Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double TotalSpent { get; set; } = 0;//Tổng số tiền đã cho tiêu
        public double DiscountRate { get; set; } = 0;
    }
}
