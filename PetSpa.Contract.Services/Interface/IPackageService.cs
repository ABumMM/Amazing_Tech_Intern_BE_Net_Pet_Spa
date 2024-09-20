
using PetSpa.Contract.Repositories.Entity;
<<<<<<< HEAD
using PetSpa.ModelViews.ModelViews;
=======
using PetSpa.Core.Base;
using PetSpa.ModelViews.PackageModelViews;
>>>>>>> 05e26ddaeb54be6bb24dbaadf87ff8883bb5426d


namespace PetSpa.Contract.Services.Interface
{
    public interface IPackageService
    {
<<<<<<< HEAD
        Task<IList<PackageResponseModel>> GetAll();
        Task<PackageResponseModel?> GetById(object id);
        Task Add(Packages package);
=======
        Task<BasePaginatedList<GETPackageViewModel>> GetAll(int pageNumber = 1, int pageSize = 3);
        Task<GETPackageViewModel?> GetById(string packageID);
        Task<List<GETPackageViewModel?>> GetPackages(string packageID, DateTime? DateStart, DateTime? EndStart,string? Name);
        Task Add(POSTPackageViewModel package);
>>>>>>> 05e26ddaeb54be6bb24dbaadf87ff8883bb5426d
        Task Update(Packages package);
        Task Delete(string id);
    }
}
