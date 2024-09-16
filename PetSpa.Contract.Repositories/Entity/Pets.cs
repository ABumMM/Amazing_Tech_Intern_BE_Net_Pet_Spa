using PetSpa.Core.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.Contract.Repositories.Entity
{
<<<<<<< HEAD
    public class Pets
=======
    public class Pets : BaseEntity
>>>>>>> 833a266402cd1da52766d54545ab44796f4257b5
    {
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
