using PetSpa.Core.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PetSpa.Contract.Repositories.Entity
{
    public class ServicesEntity : BaseEntity
    {
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public string? PackageId { get; set; }
        [JsonIgnore]
        public virtual Packages? Package { get; set; }

<<<<<<< HEAD
=======

>>>>>>> 05e26ddaeb54be6bb24dbaadf87ff8883bb5426d
    }
}

