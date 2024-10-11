using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.ModelViews.OrderDetailModelViews
{
    public class PUTOrderDetailModelView
    {
        public required string Id { get; set; }
        public int? Quantity { get; set; }
        public string? Status { get; set; }
        //public decimal Price { get; set; } = 0;
        public string? LastUpdatedBy { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
    }
}
