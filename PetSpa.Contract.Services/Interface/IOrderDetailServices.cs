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
        Task<BasePaginatedList<GETOrderDetailModelView>> getAll(int pageNumber = 1, int pageSize = 3);
        Task<GETOrderDetailModelView?> getById(string OrDetailID);
        //Task<GETOrderDetailModelView?> GETOrderDetail(string orDetailID, DateTime? DateStart, DateTime? EndStart);
        Task Add(POSTOrderDetailModelView detailMV);
        Task Update(PUTOrderDetailModelView detailsMV);
        Task Delete(string OrDetailID);

    }
}
