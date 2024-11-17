using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.ModelViews.PaymentModelView
{
    public class PostPaymentModelView
    {
        public required string OrderId { get; set; } 
        public string? Description { get; set; }
    }

}
