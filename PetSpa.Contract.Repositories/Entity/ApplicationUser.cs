using Microsoft.AspNetCore.Identity;
using PetSpa.Core.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.Contract.Repositories.Entity
{
    public class ApplicationUser:IdentityUser<Guid>
    {

        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public virtual UserInfo? UserInfo { get; set; }
        public string? CreatedBy { get; set; }
        public string? LastUpdatedBy { get; set; }
        public string? DeletedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }

        // Khóa ngoại đến ApplicationRole
        public Guid RoleId { get; set; }
        public virtual ApplicationRole Role { get; set; }

        public virtual ICollection<Pets> Pets { get; set; } = new List<Pets>();
        public ApplicationUser()
        {
            CreatedTime = CoreHelper.SystemTimeNow;
            LastUpdatedTime = CreatedTime;
        }
    }
}
