using PetSpa.Contract.Repositories.Entity;
using PetSpa.Core.Base;
using PetSpa.ModelViews.ModelViews;
using PetSpa.ModelViews.RoleModelViews;
using PetSpa.ModelViews.UserModelViews;
namespace PetSpa.Contract.Services.Interface
{
    public interface IUserService
    {
        Task<BasePaginatedList<GETUserModelView>> GetAll(int pageNumber = 1, int pageSize = 3);
        Task<GETUserModelView?> GetById(Guid userID);
        Task Update(PUTUserModelView user);
        Task Delete(Guid Id);
    }
}
