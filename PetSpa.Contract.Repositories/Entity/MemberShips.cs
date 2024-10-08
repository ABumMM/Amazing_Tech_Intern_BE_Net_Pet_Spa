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
        public int? Point { get; set; } 
        public string? SpecialOffer { get; set; }
    }
}
