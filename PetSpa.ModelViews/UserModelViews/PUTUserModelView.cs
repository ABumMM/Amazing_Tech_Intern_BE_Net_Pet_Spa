using PetSpa.Contract.Repositories.Entity;
using PetSpa.ModelViews.PetsModelViews;
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
        public GETUserInfoModelView? UserInfo { get; set; }
        public string RoleName { get; set; } = string.Empty;
    }
}
