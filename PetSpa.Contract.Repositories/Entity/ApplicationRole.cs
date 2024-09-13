using PetSpa.Core.Utils;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace PetSpa.Contract.Repositories.Entity
{
    public class ApplicationRole: IdentityRole<Guid>
    {
        public string? CreatedBy { get; set; }
        public string? LastUpdatedBy { get; set; }
        public string? DeletedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }

<<<<<<< HEAD
        // Quan hệ một-nhiều với Employees
        /*public virtual ICollection<Employees> Employees { get; set; } = new List<Employees>();
        // Quan hệ một-nhiều với Customers
        public virtual ICollection<Customers> Customers { get; set; } = new List<Customers>();*/
=======
        //// Quan hệ một-nhiều với Employees
        //public virtual ICollection<Employees> Employees { get; set; } = new List<Employees>();
        //// Quan hệ một-nhiều với Customers
        //public virtual ICollection<Customers> Customers { get; set; } = new List<Customers>();
>>>>>>> a1247fff7a39cde2f6ecd1b258c154411790114c

        public ApplicationRole()
        {
            CreatedTime = CoreHelper.SystemTimeNow;
            LastUpdatedTime = CreatedTime;
        }
    }
}
