using PetSpa.Contract.Repositories.Entity;
using PetSpa.Core.Base;
using PetSpa.ModelViews.PetsModelViews;
using PetSpa.ModelViews.UserModelViews;

namespace PetSpa.Contract.Services.Interface
{
    public interface IPetService
    {
        Task<GETPetsModelView> GetById(string petsID);
        Task<BasePaginatedList<GETPetsModelView>> GetAll(int pageNumber, int pageSize);
        Task<PUTPetsModelView> Update(PUTPetsModelView pets);
     
        Task Delete(string Id);
        Task Add(POSTPetsModelView petMV);
    }
}
