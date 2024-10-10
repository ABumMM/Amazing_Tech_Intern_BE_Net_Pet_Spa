using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Core.Base;
using PetSpa.ModelViews.ModelViews;
using PetSpa.ModelViews.OrderModelViews;
using PetSpa.ModelViews.PackageModelViews;
namespace PetSpa.Contract.Services.Interface
{
    public interface IOrderService
    {
        Task<BasePaginatedList<GetOrderViewModel>> GetAll(int pageNumber , int pageSize );
        Task<GetOrderViewModel?> GetById(string id);
        Task Add(PostOrderViewModel order);
        Task Update(PutOrderViewModel order);
        Task Delete(string id);
    }
}
