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
            _unitOfWork = unitOfWork;
        }
        public Task Add(Packages package)
        {
            throw new NotImplementedException();
        }

        public Task Delete(object id)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Packages>> GetAll()
        {
            return _unitOfWork.GetRepository<Packages>().GetAllAsync();
        }

        public Task<Packages?> GetById(object id)
        {
            throw new NotImplementedException();
        }

        public Task Update(Packages package)
        {
            throw new NotImplementedException();
        }
    }
}
