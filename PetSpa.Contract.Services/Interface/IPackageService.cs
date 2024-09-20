
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Core.Base;
using PetSpa.ModelViews.PackageModelViews;


namespace PetSpa.Contract.Services.Interface
{
    public interface IPackageService
    {
        Task<BasePaginatedList<GETPackageModelView>> GetAll(int pageNumber = 1, int pageSize = 3);
        Task<GETPackageModelView?> GetById(string packageID);
        Task<List<GETPackageModelView?>> GetPackages(string packageID, DateTime? DateStart, DateTime? EndStart,string? Name);
        Task Add(POSTPackageModelView package);
        Task Update(PUTPackageModelView packageMV);
        Task Delete(string packageID);
    }
}
