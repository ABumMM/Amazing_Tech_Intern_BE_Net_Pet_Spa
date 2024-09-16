using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Repositories.IUOW;
using PetSpa.Contract.Services.Interface;

namespace PetSpa.Services.Service
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;


        public OrderService(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }

        public async Task Add(Orders o)
        {
            o.Id = Guid.NewGuid().ToString("N");
            IGenericRepository<Orders> genericRepository = _unitOfWork.GetRepository<Orders>();
            await genericRepository.InsertAsync(o);
            await _unitOfWork.SaveAsync();
        }

        public async Task Delete(object id)
        {
            IGenericRepository<Orders> genericRepository = _unitOfWork.GetRepository<Orders>();
            await genericRepository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }

        public Task<IList<Orders>> GetAll()
        {
            return _unitOfWork.GetRepository<Orders>().GetAllAsync();
        }

        public Task<Orders?> GetById(object id)
        {
            return _unitOfWork.GetRepository<Orders>().GetByIdAsync(id);
        }

        public async Task Update(Orders o)
        {
            IGenericRepository<Orders> genericRepository = _unitOfWork.GetRepository<Orders>();
            await genericRepository.UpdateAsync(o);
            await _unitOfWork.SaveAsync();
        }
    }
}
