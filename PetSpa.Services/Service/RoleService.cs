using PetSpa.Contract.Repositories.IUOW;
using PetSpa.Repositories.UOW;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetSpa.Contract.Services.Interface;
using PetSpa.Contract.Repositories.Entity;

namespace PetSpa.Services.Service
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<ApplicationRole> GetByIdAsync(Guid id)
        {
            return await _unitOfWork.GetRepository<ApplicationRole>().GetByIdAsync(id);
        }

        public async Task<IEnumerable<ApplicationRole>> GetAllAsync()
        {
            return await _unitOfWork.GetRepository<ApplicationRole>().GetAllAsync();
        }

        public async Task AddAsync(ApplicationRole role)
        {
            await _unitOfWork.GetRepository<ApplicationRole>().InsertAsync(role);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateAsync(ApplicationRole role)
        {
            _unitOfWork.GetRepository<ApplicationRole>().Update(role);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            await _unitOfWork.GetRepository<ApplicationRole>().DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }
    }
}
