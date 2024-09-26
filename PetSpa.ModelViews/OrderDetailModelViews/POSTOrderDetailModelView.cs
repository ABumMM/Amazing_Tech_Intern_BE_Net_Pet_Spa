using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.ModelViews.OrderDetailModelViews
{
    public class POSTOrderDetailModelView
    {
        public int? Quantity { get; set; }
        public string? Status { get; set; }
        public decimal? Price { get; set; }
        public List<string>? PackageIDs { get; set; }
        public string? CreatedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
    }
}
