using PetSpa.Core.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.Contract.Repositories.Entity
{
    public class Services : BaseEntity
    {
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
    }
}

