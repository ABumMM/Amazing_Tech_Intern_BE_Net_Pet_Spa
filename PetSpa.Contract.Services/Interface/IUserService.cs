using PetSpa.Core.Base;
using PetSpa.ModelViews.UserModelViews;

public interface IUserService
{
    Task<GETUserModelView> GetById(Guid id);
    Task<BasePaginatedList<GETUserModelView>> GetAll(int pageNumber, int pageSize);
    Task<BasePaginatedList<GETUserModelView>> GetCustomers(int pageNumber, int pageSize);
    Task<BasePaginatedList<GETUserModelView>> GetEmployees(int pageNumber, int pageSize);
    Task<PUTUserModelView> Update(PUTUserModelView user);
    Task<PUTuserforcustomer> UpdateForCustomer(PUTuserforcustomer customer);
    Task Delete(Guid id);
}
