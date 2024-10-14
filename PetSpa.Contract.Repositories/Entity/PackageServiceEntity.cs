using PetSpa.Core.Base;
namespace PetSpa.Contract.Repositories.Entity
{
    public class PackageServiceEntity : BaseEntity
    {
        public string? PackageId { get; set; }
        public virtual Packages? Package { get; set; }
        public string? ServicesEntityID { get; set; }
        public virtual Services? ServicesEntity { get; set; }
       

    }
}
