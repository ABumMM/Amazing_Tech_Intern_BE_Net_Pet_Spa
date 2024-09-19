
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Core.Base;
using PetSpa.ModelViews.PackageModelViews;


namespace PetSpa.Contract.Services.Interface
{
    public interface IPackageService
    {
        Task<BasePaginatedList<GETPackageViewModel>> GetAll(int pageNumber = 1, int pageSize = 3);
        Task<GETPackageViewModel?> GetById(string packageID);
        Task<List<GETPackageViewModel?>> GetPackages(string packageID, DateTime? DateStart, DateTime? EndStart,string? Name);
        Task Add(POSTPackageViewModel package);
        Task Update(Packages package);
        Task Delete(string id);
    }
}
