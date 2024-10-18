using PetSpa.Core.Base;
using PetSpa.ModelViews.MemberShipModelView;

namespace PetSpa.Contract.Services.Interface
{
    public interface IMembershipsService
    {
        Task<BasePaginatedList<GETMemberShipModelView>> GetAll(int pageNumber, int pageSize);
        Task<GETMemberShipModelView?> GetById(string memberShipID);
        Task UpdateMemberShip(string OrderID);
    }
}
