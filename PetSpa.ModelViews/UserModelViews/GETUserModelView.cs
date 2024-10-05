using PetSpa.Contract.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.ModelViews.UserModelViews
{
    public class GETUserModelView
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public UserInfo? UserInfo { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public string? CreatedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
    }
}
