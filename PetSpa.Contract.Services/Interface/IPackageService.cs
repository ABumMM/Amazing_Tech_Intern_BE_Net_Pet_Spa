
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Core.Base;
using PetSpa.ModelViews.PackageModelViews;
using PetSpa.ModelViews.PackageServiceModelViews;


namespace PetSpa.Contract.Services.Interface
{
    public interface IPackageService
    {
        Task<BasePaginatedList<GETPackageModelView>> GetAll(int pageNumber, int pageSize);
        Task<GETPackageModelView?> GetById(string packageID);
        Task<List<GETPackageServiceModelView>> GetServicesByPackageId(string packageId);
        Task<List<GETPackageModelView>?> GetPackageByConditions(DateTimeOffset? DateStart, DateTimeOffset? EndStart);
        Task Add(POSTPackageModelView package);
        Task AddServiceInPackage(string packageID, string serviceID);
        Task Update(PUTPackageModelView packageMV);
        Task Delete(string packageID);
        Task DeleteServiceInPakcage(string serviceINPackageID);

    }
}
