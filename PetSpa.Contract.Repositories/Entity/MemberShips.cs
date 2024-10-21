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
        public decimal TotalSpent { get; set; } = 0;//Tổng số tiền đã cho tiêu
        public required string RankId { get; set; }
        public Guid UserId { get; set; }
        public virtual ApplicationUser? User { get; set; } 

        public virtual Rank? Rank { get; set; }

    }
}
