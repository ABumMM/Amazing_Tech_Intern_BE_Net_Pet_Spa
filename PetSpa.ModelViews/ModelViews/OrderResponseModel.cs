using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.ModelViews.ModelViews
{
    public class OrderResponseModel
    {
        public string Id { get; set; }
        public Guid? CustomerID { get; set; }
        public Guid? EmployeeID { get; set; }
        public DateTime Date { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public double Total { get; set; }
    }
}
