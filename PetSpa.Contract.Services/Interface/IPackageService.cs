
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Core.Base;
using PetSpa.ModelViews.ModelViews;


namespace PetSpa.Contract.Services.Interface
{
    public interface IPackageService
    {
        Task<BasePaginatedList<PackageResponseModel>> GetAll(int pageNumber = 1, int pageSize = 3);
        Task<PackageResponseModel?> GetById(Guid id);
        Task Add(Packages package);
        Task Update(Packages package);
        Task Delete(Guid id);
    }
}
