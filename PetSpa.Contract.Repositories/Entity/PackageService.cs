using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.Contract.Repositories.Entity
{
    public class PackageService
    {
        public string? PackageId { get; set; }
        public virtual Packages? Package { get; set; }
        public string? ServiceId { get; set; }
        public virtual ServicesEntity? ServicesEntity { get; set; }

        public DateTimeOffset AddedDate { get; set; } // Ngày thêm service vào package
    }
}
