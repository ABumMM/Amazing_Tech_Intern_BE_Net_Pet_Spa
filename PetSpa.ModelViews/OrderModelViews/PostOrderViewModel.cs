using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.ModelViews.OrderModelViews
{
    public class PostOrderViewModel
    {
        public string PaymentMethod { get; set; } = "Unknown";
        public double Total { get; set; }

        public DateTimeOffset CreatedTime { get; set; }
    }
}
