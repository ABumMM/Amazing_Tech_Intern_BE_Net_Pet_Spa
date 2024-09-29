using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.ModelViews.ModelViews
{
    public class OrderResponseModel
    {
        public string? Id { get; set; }
        public string? CustomerID { get; set; }
        public string? EmployeeID { get; set; }
        public DateTime? Date { get; set; }
        public string? PaymentMethod { get; set; }
        public decimal? Total { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedTime { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedTime { get; set; }
    }
}
