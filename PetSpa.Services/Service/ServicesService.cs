/*using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Repositories.IUOW;
using PetSpa.Contract.Services.Interface;
using PetSpa.ModelViews.ServiceModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.Services.Service
{
    public class ServicesService : IServicesService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServicesService(IUnitOfWork unitOfWork) { 
            _unitOfWork = unitOfWork;
        }
        public async Task Add(ServicesEntity  service)
        {
           
           
            
            IGenericRepository<ServicesEntity> genericRepository = _unitOfWork.GetRepository<ServicesEntity>();
            await genericRepository.InsertAsync(service);
            await _unitOfWork.SaveAsync();
        }

        public async Task Delete(object id)
        {
            IGenericRepository<ServicesEntity> genericRepository = _unitOfWork.GetRepository<ServicesEntity>();
            await genericRepository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }

        public Task<IList<ServicesEntity>> GetAll()
        {
            return _unitOfWork.GetRepository<ServicesEntity>().GetAllAsync();
        }

        public Task<ServicesEntity?> GetById(object id)
        {
            return _unitOfWork.GetRepository<ServicesEntity>().GetByIdAsync(id);
        }

        public async Task Update(ServicesEntity service)
        {   
            IGenericRepository<ServicesEntity> genericRepository = _unitOfWork.GetRepository<ServicesEntity>();
            await genericRepository.UpdateAsync(service);
            await _unitOfWork.SaveAsync();
        }
    }
}
*/