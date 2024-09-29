using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.ModelViews.MemberShipModelViews
{
    public class PUTMemberShipModelView
    {
        public string? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int? Point { get; set; }
        public string? SpecialOffer { get; set; }
        public string? LastUpdatedBy { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
    }
}
