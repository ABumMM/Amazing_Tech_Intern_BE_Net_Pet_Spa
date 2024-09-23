using PetSpa.Core.Base;
using PetSpa.ModelViews.PackageModelViews;
using PetSpa.ModelViews.RoleModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.Contract.Services.Interface
{
    public interface IRoleService
    {
        Task<BasePaginatedList<GETRoleModelView>> GetAll(int pageNumber = 1, int pageSize = 3);
        Task<GETRoleModelView?> GetById(Guid Id);
        Task Add(POSTRoleModelView role);
        Task Update(PUTRoleModelView role);
        Task Delete(Guid Id);
    }
}
