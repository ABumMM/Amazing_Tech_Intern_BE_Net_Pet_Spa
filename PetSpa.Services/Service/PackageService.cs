using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Repositories.IUOW;
using PetSpa.Contract.Services.Interface;
using PetSpa.ModelViews.ModelViews;
namespace PetSpa.Services.Service
{
    public class PackageService : IPackageService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PackageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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

        public async Task<IList<PackageResponseModel>> GetAll()
        {
            var packages = await _unitOfWork.GetRepository<Packages>().GetAllAsync();
            var packageResponseModels = packages.Select(pa => new PackageResponseModel
            {
                Id = pa.Id.ToString(),
                Name=pa.Name,
                Image=pa.Image,
                Information=pa.Information,
                Experiences=pa.Experiences,
                ServiceEntityResponseModels = pa.Service.Select(se => new ServiceEntityResponseModel
                {
                    Id = se.Id.ToString(),
                    Name = se.Name,
                    // Ánh xạ các thuộc tính khác nếu cần
                }).ToList()
            }).ToList();

            return packageResponseModels;
        }

        public Task<PackageResponseModel?> GetById(object id)
        {
            return _unitOfWork.GetRepository<PackageResponseModel>().GetByIdAsync(id);
        }

        public async Task Update(Packages package)
        {
            IGenericRepository<Packages> genericRepository = _unitOfWork.GetRepository<Packages>();
            await genericRepository.UpdateAsync(package);
            await _unitOfWork.SaveAsync();
        }
    }
}
