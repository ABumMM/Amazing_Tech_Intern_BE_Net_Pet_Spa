        using System;
        using System.Collections.Generic;
        using System.Linq;
        using System.Text;
        using System.Threading.Tasks;

        namespace PetSpa.ModelViews.PaymentModelView
        {
            public class GetPaymentModelView
            {
                public string Id { get; set; } = string.Empty; 
                public string PaymentMethod { get; set; } = string.Empty;
                public DateTime PaymentDate { get; set; }
                public string? Description {  get; set; }
                public bool Successful { get; set; }
                public string TransactionId { get; set; } = string.Empty;
                public string Token { get; set; } = string.Empty;
                public string VnPayResponseCode { get; set; } = string.Empty ;

                public string? OrderId { get; set; }
            }

        }
