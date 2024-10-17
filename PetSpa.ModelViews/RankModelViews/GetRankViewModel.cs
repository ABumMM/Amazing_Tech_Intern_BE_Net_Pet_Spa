using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.ModelViews.RankModelViews
{
    public class GetRankViewModel
    {
        public string Id { get; set; }
        public required string Name { get; set; }
        public required int MinPrice { get; set; } // số tiền tối thiểu để đạt được mức rank đó
        public required int DiscountPercent { get; set; }
    }
}
