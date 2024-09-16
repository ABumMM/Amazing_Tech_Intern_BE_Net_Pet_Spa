using PetSpa.Contract.Repositories.Entity;
using PetSpa.ModelViews.ModelViews;
namespace PetSpa.Contract.Services.Interface
{
    public interface IUserService
    {
        Task<IList<UserResponseModel>> GetAll();
        Task<UserResponseModel?> GetById(object id);
    }
}
