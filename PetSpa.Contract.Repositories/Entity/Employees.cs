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
        [Key]
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNPhoneNumber {  get; set; }
        public DateTime DayofBirth { get; set; }
        public string Address { get; set; }
        public double? Salary { get; set; }

        //thêm khóa ngoại sau
    }
}
