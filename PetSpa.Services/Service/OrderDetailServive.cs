using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Repositories.IUOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.Services.Service
{
    public class OrderDetailServive
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderDetailServive(IUnitOfWork unitOfWork) 
        { 
            _unitOfWork = unitOfWork;
        }

        public async Task Add(OrdersDetails ordersDetails)
        {
            ordersDetails.Id = Guid.NewGuid().ToString("");
            IGenericRepository<OrdersDetails> genericRepository = _unitOfWork.GetRepository<OrdersDetails>();
            await genericRepository.InsertAsync(ordersDetails);
            await _unitOfWork.SaveAsync();
        }

        public async Task Delete(object id)
        {
            IGenericRepository<OrdersDetails> genericRepository = _unitOfWork.GetRepository<OrdersDetails>();
            await genericRepository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }

        public Task<IList<OrdersDetails>> GetAll()
        {
            return _unitOfWork.GetRepository<OrdersDetails>().GetAllAsync();
        }

        public Task<OrdersDetails?> GetById(object id)
        {
            return _unitOfWork.GetRepository<OrdersDetails>().GetByIdAsync(id);
        }

        public async Task Update(OrdersDetails ordersDetails)
        {
            IGenericRepository<OrdersDetails> genericRepository = _unitOfWork.GetRepository<OrdersDetails>();
            await genericRepository.UpdateAsync(ordersDetails);
            await _unitOfWork.SaveAsync();
        }
    }
}
