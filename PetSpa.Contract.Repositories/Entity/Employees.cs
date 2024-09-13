using PetSpa.Core.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.Contract.Repositories.Entity
{
    public class Employees : BaseEntity
    {
     
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNPhoneNumber {  get; set; } = string.Empty ;
        public DateTime DayofBirth { get; set; }
        public string Address { get; set; }
        public double? Salary { get; set; }

        //// Mối quan hệ với bảng Bookings(một-nhiều)
        //public virtual ICollection<Bookings> Bookings { get; set; } = new List<Bookings>();

        //// Khóa ngoại
        //public Guid ApplicationRoleId { get; set; }
        //public virtual ApplicationRole ApplicationRole { get; set; }

    }
}
