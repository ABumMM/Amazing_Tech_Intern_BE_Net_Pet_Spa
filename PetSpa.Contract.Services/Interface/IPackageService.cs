
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Core.Base;
using PetSpa.ModelViews.PackageModelViews;


namespace PetSpa.Contract.Services.Interface
{
    public interface IPackageService
    {
        Task<BasePaginatedList<GETPackageModelView>> GetAll(int pageNumber, int pageSize);
        Task<GETPackageModelView?> GetById(string packageID);
        Task<List<GETPackageModelView>?> GetPackageByConditions(DateTimeOffset? DateStart, DateTimeOffset? EndStart);
        Task Add(POSTPackageModelView package);
        Task Update(PUTPackageModelView packageMV);
        Task Delete(string packageID);
        Task DeleteServiceInPakcage(string serviceINPackageID);

    }
}
