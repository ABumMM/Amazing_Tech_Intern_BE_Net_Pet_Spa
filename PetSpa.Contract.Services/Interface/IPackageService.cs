
using PetSpa.Contract.Repositories.Entity;
using PetSpa.ModelViews.ModelViews;


namespace PetSpa.Contract.Services.Interface
{
    public interface IPackageService
    {
        Task<IList<PackageResponseModel>> GetAll();
        Task<PackageResponseModel?> GetById(object id);
        Task Add(Packages package);
        Task Update(Packages package);
        Task Delete(object id);
    }
}
