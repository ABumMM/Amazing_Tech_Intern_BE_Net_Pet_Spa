using PetSpa.Core.Base;
using PetSpa.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.Contract.Repositories.Entity
{
    public class Payment : BaseEntity
    {
        public string PaymentMethod { get; set; } = string.Empty;
        public DateTimeOffset PaymentDate { get; set; } = TimeHelper.ConvertToUtcPlus7(DateTime.Now);
        public bool Successful { get; set; }
        public string TransactionId { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Token { get; set; } = string.Empty;
        public string VnPayResponseCode { get; set; } = string.Empty;
        public required string OrderId { get; set; }
        public required virtual Orders Order { get; set; }
    }
}
