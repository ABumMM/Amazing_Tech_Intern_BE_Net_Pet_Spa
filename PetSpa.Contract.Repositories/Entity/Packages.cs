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
        [Key]
        public Guid ID {  get; set; }
        public string Name { get; set; }=String.Empty;
        public string? Image {  get; set; }
        public string? Information {  get; set; }
        public string? Experiences {  get; set; }
<<<<<<< HEAD
        public  ICollection<Services>?Services { get; set; }
=======
        public Guid ServiceID { get; set; }
<<<<<<< HEAD
=======

>>>>>>> 3624f926008b220ca2844f490a7666c996e0ee1e
        public virtual ICollection<Services>? Services { get; set; }

>>>>>>> ff5a444a0cbb6a05ab6f290c552d05cb92909499
    }
}
