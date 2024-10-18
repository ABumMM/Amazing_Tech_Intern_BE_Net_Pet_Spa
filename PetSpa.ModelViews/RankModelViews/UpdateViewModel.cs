using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.ModelViews.RankModelViews
{
    public class UpdateViewModel
    {
        public required string Name { get; set; }
        public required int MinPrice { get; set; } 
        public required int DiscountPercent { get; set; }
    }
}
