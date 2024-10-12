using PetSpa.Core.Base;
namespace PetSpa.Contract.Repositories.Entity
{
    public class Reviews : BaseEntity
    {
        public string Description { get; set; }=string.Empty;
        public string ?PackageID { get; set; }
        public virtual Packages? Package { get; set; }
    }
}
