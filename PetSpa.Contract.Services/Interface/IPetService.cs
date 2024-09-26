using PetSpa.Contract.Repositories.Entity;
using PetSpa.Core.Base;
using PetSpa.ModelViews.PetsModelViews;

namespace PetSpa.Contract.Services.Interface
{
    public interface IPetService
    {
        Task<BasePaginatedList<GETPetsModelView>> GetAll(int pageNumber = 1, int pageSize = 3);
        Task<GETPetsModelView?> GetById(string Id);
        Task Add(POSTPetsModelView petMV);
        Task Update(PUTPetsModelView petMV);
        Task Delete(string Id);
    }
}
