using PetSpa.Contract.Repositories.Entity;
using PetSpa.Core.Base;
using PetSpa.ModelViews.OrderDetailModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.Contract.Services.Interface
{
    public interface IOrderDetailServices
    {
        Task<BasePaginatedList<GETOrderDetailModelView>> GetAll(int pageNumber, int pageSize);
        Task<GETOrderDetailModelView?> GetById(string OrDetailID);
        Task<List<GETOrderDetailModelView>?> GETOrderDetailByConditions(DateTimeOffset? DateStart, DateTimeOffset? DateEnd);
        Task Add(POSTOrderDetailModelView detailsMV);
        Task<decimal> CalculateTotalPrice(string orderId);
        Task UpdateOrderTotal(string orderId);
        Task Update(PUTOrderDetailModelView detailsMV);
        Task Delete(string OrDetailID);
    }
}
