using PetSpa.ModelViews.PackageModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.ModelViews.OrderDetailModelViews
{
    public class GETOrderDetailModelView
    {
        public required string Id { get; set; }
        public int? Quantity { get; set; }
        public string? Status { get; set; }
        public decimal Price { get; set; } = 0; 
        public required string OrderID { get; set; }
        public string? CreatedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public string? LastUpdatedBy { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
        public string? DeletedBy { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }


    }
}
