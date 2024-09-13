using PetSpa.Core.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.Contract.Repositories.Entity
{
    public class Pets : BaseEntity
    {
<<<<<<< HEAD
=======
   
>>>>>>> a1247fff7a39cde2f6ecd1b258c154411790114c
        public string Name { get; set; } = string.Empty;
        public string Species { get; set; } = string.Empty;
        public decimal Weight { get; set; }
        public string Breed { get; set; } 
        public int Age { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public int CustomerID { get; set; } 

    }
}
