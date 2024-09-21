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
        Task<ServicesEntity?> GetById(object id);
        Task Add(ServicesEntity service);
        Task Update(ServicesEntity service);
        Task Delete(object id);
    }
}
