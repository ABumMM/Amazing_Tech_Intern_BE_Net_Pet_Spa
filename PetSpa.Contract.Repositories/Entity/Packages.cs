using Microsoft.VisualBasic;
using PetSpa.Core.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace PetSpa.Contract.Repositories.Entity
{
    public class Packages: BaseEntity
    {
        public string Name { get; set; }=String.Empty;
        public string? Image {  get; set; }
        public string? Information {  get; set; }
        public string? Experiences {  get; set; }

        /*public  ICollection<Services>?Service { get; set; }
        public Guid ServiceID { get; set; }
        public virtual ICollection<Services>? Services { get; set; }*/
    }
}
