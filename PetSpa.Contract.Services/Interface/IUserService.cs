using PetSpa.Contract.Repositories.Entity;
using PetSpa.Core.Base;
using PetSpa.ModelViews.ModelViews;
using PetSpa.ModelViews.RoleModelViews;
using PetSpa.ModelViews.UserModelViews;
namespace PetSpa.Contract.Services.Interface
{
    public interface IUserService
    {
        Task<GETUserModelView> GetById(Guid id);
        Task<BasePaginatedList<GETUserModelView>> GetAll(int pageNumber, int pageSize);
        Task<PUTUserModelView> Update(PUTUserModelView user);
        Task Delete(Guid Id);
    }
}
