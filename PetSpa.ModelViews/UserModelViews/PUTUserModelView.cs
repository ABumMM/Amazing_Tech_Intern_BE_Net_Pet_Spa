using PetSpa.Contract.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.ModelViews.UserModelViews
{
    public class PUTUserModelView
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public UserInfo? UserInfo { get; set; }
        public string RoleName { get; set; } = string.Empty;

        public string? LastUpdatedBy { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
    }
}
