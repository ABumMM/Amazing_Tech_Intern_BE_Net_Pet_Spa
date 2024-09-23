using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.ModelViews.PetsModelViews
{
    internal class POSTPetsModelView
    {
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Species { get; set; } = string.Empty;

        [Required]
        public decimal Weight { get; set; }

        [Required]
        public string? Breed { get; set; }

        [Required]
        public int Age { get; set; }

        public string? Description { get; set; }

        public string? Image { get; set; }

        [Required]
        public int CustomerID { get; set; }
    }
}
