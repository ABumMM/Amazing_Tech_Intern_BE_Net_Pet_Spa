using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.ModelViews.MemberShipModelViews
{
    public class POSTMemberShipModelView
    {
        public string Name { get; set; } = string.Empty;
        public int? Point { get; set; }
        public string? SpecialOffer { get; set; }
        public string? CreatedBy { get; set; }
        public DateTimeOffset? CreatedTime { get; set; }

    }
}
