using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.ModelViews.ModelViews;
namespace PetSpa.Contract.Services.Interface
{
    public interface IOrderService
    {
        Task<IList<OrderResponseModel>> GetAll();
        Task<OrderResponseModel?> GetById(object id);
        Task Add(OrderResponseModel order);
        Task Update(OrderResponseModel order);
        Task Delete(object id);
    }
}
