using PetSpa.ModelViews.UserModelViews;
namespace PetSpa.Contract.Services.Interface
{
    public interface IUserService
    {
        Task<IList<UserResponseModel>> GetAll();
    }
}
