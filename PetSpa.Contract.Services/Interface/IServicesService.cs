using PetSpa.Contract.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetSpa.ModelViews.ServiceModelViews;
using PetSpa.Core.Base;
namespace PetSpa.Contract.Services.Interface
{
    public interface IServicesService
    {
        Task<BasePaginatedList<ServiceResposeModel>> GetAll(int pageNumber = 1, int pageSize = 10);
        Task<ServiceResposeModel?> GetById(object id);
        Task Add(ServiceCreateModel service);
        Task Update(ServiceUpdateModel service);
        Task Delete(string id);
    }
}
