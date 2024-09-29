using PetSpa.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.Contract.Repositories.Entity
{
    public class OrderDetailPackage:BaseEntity
    {
        public string OrderDetailId { get; set; }
        public OrdersDetails OrderDetail { get; set; }

        public string PackageId { get; set; }
        public Packages Package { get; set; }
    }
}
