using PetSpa.Contract.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.ModelViews.PackageServiceModelViews
{
    public class GETPackageServiceModelView
    {
        public string? Id {  get; set; }
        public string? PackageId { get; set; }
        public string? ServiceId { get; set; }
        public string? CreatedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }

    }
}
