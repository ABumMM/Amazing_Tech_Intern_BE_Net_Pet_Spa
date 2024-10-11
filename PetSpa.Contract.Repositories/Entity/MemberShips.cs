using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using PetSpa.Core.Base;

namespace PetSpa.Contract.Repositories.Entity
{
    public class MemberShips : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public double TotalSpent { get; set; } = 0;//Tổng số tiền đã cho tiêu
        public double DiscountRate { get; set; } = 0;
        public Guid? UserId { get; set; }
        public virtual ApplicationUser? User { get; set; } 

    }
}
