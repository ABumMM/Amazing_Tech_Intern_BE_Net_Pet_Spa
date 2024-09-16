using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Repositories.IUOW;
using PetSpa.Contract.Services.Interface;

namespace PetSpa.Services.Service
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;


        public async OrderService(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }

        public async Task Add(Orders o)
        {
            o.Id = Guid.NewGuid().ToString("N");
            var genericRepository = _unitOfWork.GetRepository<Orders >();
            await genericRepository.InsertAsync(o);
            await _unitOfWork.SaveAsync();
        }

        public async Task Delete(object id)
        {
            var genericRepository = _unitOfWork.GetRepository<Orders>();
            await genericRepository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IList<Orders>> GetAll()
        {
            return _unitOfWork.GetRepository<Orders>().GetAllAsync();
        }

        public async Task<Orders?> GetById(object id)
        {
            return _unitOfWork.GetRepository<Orders>().GetByIdAsync(id);
        }

        public async Task Update(Orders o)
        {
            var genericRepository = _unitOfWork.GetRepository<Orders>();
            await genericRepository.UpdateAsync(o);
            await _unitOfWork.SaveAsync();
        }
    }
}
