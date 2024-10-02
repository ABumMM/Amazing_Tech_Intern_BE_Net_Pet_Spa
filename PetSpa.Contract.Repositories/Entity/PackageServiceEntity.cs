using PetSpa.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.Contract.Repositories.Entity
{
    public class PackageServiceEntity:BaseEntity
    {
        public string? PackageId { get; set; }
        public virtual Packages? Package { get; set; }
        public string? ServicesEntityID { get; set; }
        public virtual ServicesEntity? ServicesEntity { get; set; }
       

    }
}
