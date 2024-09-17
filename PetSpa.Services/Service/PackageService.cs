using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Repositories.IUOW;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
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
            await _unitOfWork.GetRepository<Packages>().InsertAsync(package);
            await _unitOfWork.SaveAsync();
        }

        public async Task Delete(Guid id)
        {
            await _unitOfWork.GetRepository<Packages>().DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }

        public async Task<BasePaginatedList<PackageResponseModel>> GetAll(int pageNumber = 1, int pageSize = 2)
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

            //Count Package
            int totalPackage = packages.Count;
            return new BasePaginatedList<PackageResponseModel>(packageResponseModels, totalPackage, pageNumber, pageSize);
        }

        public Task<PackageResponseModel?> GetById(Guid id)
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
