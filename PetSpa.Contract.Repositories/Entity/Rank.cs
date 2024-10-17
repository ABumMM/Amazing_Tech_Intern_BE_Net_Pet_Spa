using PetSpa.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.Contract.Repositories.Entity
{
    public class Rank : BaseEntity
    {
        public required string Name { get; set; }
        public required int MinPrice { get; set; } // số tiền tối thiểu để đạt được mức rank đó
        public required int DiscountPercent { get; set; }
        public ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
    }
}
