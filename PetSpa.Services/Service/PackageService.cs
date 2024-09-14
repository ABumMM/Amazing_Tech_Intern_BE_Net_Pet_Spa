using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Repositories.IUOW;
using PetSpa.Contract.Services.Interface;
namespace PetSpa.Services.Service
{
    public class PackageService : IPackageService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PackageService(IUnitOfWork unitOfWork) 
        {
            _unitOfWork=unitOfWork;
        }
        public async Task Add(Packages package)
        {
            package.Id = Guid.NewGuid().ToString("N");
            IGenericRepository<Packages> genericRepository = _unitOfWork.GetRepository<Packages>();
            await genericRepository.InsertAsync(package);
            await _unitOfWork.SaveAsync();
        }

        public async Task Delete(object id)
        {
            IGenericRepository<Packages> genericRepository = _unitOfWork.GetRepository<Packages>();
            await genericRepository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }

        public Task<IList<Packages>> GetAll()
        {
            return _unitOfWork.GetRepository<Packages>().GetAllAsync();
        }

        public Task<Packages?> GetById(object id)
        {
            return _unitOfWork.GetRepository<Packages>().GetByIdAsync(id);
        }

        public async Task Update(Packages package)
        {
            IGenericRepository<Packages> genericRepository = _unitOfWork.GetRepository<Packages>();
            await genericRepository.UpdateAsync(package);
            await _unitOfWork.SaveAsync();
        }
    }
}
