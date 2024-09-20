using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.ModelViews.ServiceModelViews
{
    public class ServiceResposeModel
    {
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public string? PackageId { get; set; }
        //public string? PackageName { get; set; }
    }
}
