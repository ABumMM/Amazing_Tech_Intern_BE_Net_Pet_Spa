using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.ModelViews.OrderDetailModelViews
{
    public class POSTOrderDetailModelView
    {
        public int Quantity { get; set; } = 0;
        public string? Status { get; set; }
        public required string OrderID { get; set; }

        public string? PackageID { get; set; } // Danh sách PackageID

        public string? CreatedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
    }
}