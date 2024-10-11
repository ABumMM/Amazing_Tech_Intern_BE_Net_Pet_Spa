using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.ModelViews.RoleModelViews
{
    public class POSTRoleModelView
        
    {
        public string Name { get; set; } = string.Empty;
        public string? CreatedBy { get; set; }
        public Guid UserId { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
    }
}
