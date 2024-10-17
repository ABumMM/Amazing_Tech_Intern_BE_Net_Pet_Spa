using PetSpa.ModelViews.OrderDetailModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.ModelViews.OrderModelViews
{
    public class GetOrderViewModel
    {
        //public string? UserId { get; set; }
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? PaymentMethod { get; set; }
        public double? Total { get; set; }
        public bool IsPaid { get; set; }
        public List<string>? OrderDetailId { get; set; }
        public string? CreatedBy { get; set; }
        public DateTimeOffset? CreatedTime { get; set; }
        public string? CustomerID { get; set; }
    }
}
