using PetSpa.Contract.Repositories.Entity;
namespace PetSpa.Contract.Services.Interface
{
    public interface IMembershipsService
    {
        Task<IList<MemberShips>> GetAll();
        Task<MemberShips?> GetById(object id);
        Task Add(MemberShips memberShip);
        Task Update(MemberShips memberShip);
        Task Delete(object id);
    }
}
