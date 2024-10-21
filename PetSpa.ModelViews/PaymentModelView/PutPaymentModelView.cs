using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.ModelViews.PaymentModelView
{
    public class PutPaymentModelView
    {
        public string PaymentMethod { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public bool IsSuccessful { get; set; }
        public string TransactionId { get; set; } = string.Empty;
    }
}
