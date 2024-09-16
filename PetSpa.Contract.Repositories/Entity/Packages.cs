
using PetSpa.Core.Base;
using System.Text.Json.Serialization;


namespace PetSpa.Contract.Repositories.Entity
{
    public class Packages: BaseEntity
    {
        public string Name { get; set; }=String.Empty;
        public string? Image {  get; set; }
        public string? Information {  get; set; }
        public string? Experiences {  get; set; }

        public  ICollection<Services>?Services { get; set; }

        public Guid ServiceID { get; set; }

        public virtual ICollection<Services>? Services { get; set; }


        [JsonIgnore]
        public ICollection<Services>? Service { get; set; }
        //public Guid ServiceID { get; set; }
        //public virtual ICollection<Services>? Services { get; set; }
    }
}
