
using PetSpa.Contract.Repositories.Entity;


namespace PetSpa.Contract.Services.Interface
{
    public interface IPackageService
    {
        Task<IList<Packages>> GetAll();
        Task<Packages?> GetById(object id);
        Task Add(Packages package);
        Task Update(Packages package);
        Task Delete(object id);
    }
}
