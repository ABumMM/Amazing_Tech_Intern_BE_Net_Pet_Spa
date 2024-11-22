using PetSpa.Core.Base;
namespace PetSpa.Contract.Repositories.Entity
{
    public class PackageServiceEntity : BaseEntity
    {
        public required string PackageId { get; set; }
        public virtual Packages? Package { get; set; }
        public required string ServicesEntityID { get; set; }
        public virtual Services? ServicesEntity { get; set; }
       

    }
}
