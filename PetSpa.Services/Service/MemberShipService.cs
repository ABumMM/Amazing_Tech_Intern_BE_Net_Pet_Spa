using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Repositories.IUOW;
using PetSpa.Contract.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.Services.Service
{
    public class MemberShipService:IMembershipsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MemberShipService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task Add(MemberShips memberShip)
        {
            memberShip.Id = Guid.NewGuid().ToString("N");
            IGenericRepository<MemberShips> genericRepository = _unitOfWork.GetRepository<MemberShips>();
            await genericRepository.InsertAsync(memberShip);
            await _unitOfWork.SaveAsync();
        }

        public async Task Delete(object id)
        {
            IGenericRepository<MemberShips> genericRepository = _unitOfWork.GetRepository<MemberShips>();
            await genericRepository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }

        public Task<IList<MemberShips>> GetAll()
        {
            return _unitOfWork.GetRepository<MemberShips>().GetAllAsync();
        }

        public Task<MemberShips?> GetById(object id)
        {
            return _unitOfWork.GetRepository<MemberShips>().GetByIdAsync(id);
        }

        public async Task Update(MemberShips memberShip)
        {
            IGenericRepository<MemberShips> genericRepository = _unitOfWork.GetRepository<MemberShips>();
            await genericRepository.UpdateAsync(memberShip);
            await _unitOfWork.SaveAsync();
        }
    }
}
