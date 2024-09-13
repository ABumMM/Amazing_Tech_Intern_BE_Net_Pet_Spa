using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using PetSpa.Core.Base;

namespace PetSpa.Contract.Repositories.Entity
{
    public class MemberShip : BaseEntity
    {
<<<<<<< HEAD
=======
  

>>>>>>> a1247fff7a39cde2f6ecd1b258c154411790114c
        public string Name { get; set; } = string.Empty;

        public DateTime Date { get; set; }

        public int Point { get; set; } 

        public string SpecialOffer { get; set; }
    }
}
