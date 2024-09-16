using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetSpa.Contract.Repositories.Entity;

namespace PetSpa.Contract.Services.Interface
{
    public interface IOrderDetailService
    {
        Task<IList<OrdersDetails>> getAll();
        Task<OrdersDetails?> getById(object id);
        Task Add(OrdersDetails details);
        Task Update(OrdersDetails details);
        Task Delete(object id);
    }
}
