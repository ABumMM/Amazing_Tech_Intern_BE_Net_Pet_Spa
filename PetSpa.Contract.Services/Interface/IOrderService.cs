using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetSpa.Contract.Repositories.Entity;

namespace PetSpa.Contract.Services.Interface
{
    public interface IOrderService
    {
        Task<IList<Orders>> GetAll();
        Task<Orders?> GetById(object id);
        Task Add(Orders o);
        Task Update(Orders o);
        Task Delete(object id);
    }
}
