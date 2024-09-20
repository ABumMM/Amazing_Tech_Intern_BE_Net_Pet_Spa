
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Repositories.IUOW;
using PetSpa.Contract.Services.Interface;
<<<<<<< HEAD
using PetSpa.ModelViews.ModelViews;
using PetSpa.ModelViews.UserModelViews;
=======
using PetSpa.Core.Base;
using PetSpa.ModelViews.ModelViews;
using PetSpa.ModelViews.PackageModelViews;
using PetSpa.Repositories.UOW;

>>>>>>> 05e26ddaeb54be6bb24dbaadf87ff8883bb5426d
namespace PetSpa.Services.Service
{
    public class PackageService : IPackageService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PackageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task Add(POSTPackageViewModel packageVM)
        {
            if (packageVM == null)
            {
                throw new BadRequestException(ErrorCode.BadRequest, "Package cannot null.");
            }
            if (packageVM.Name==null)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest,ErrorCode.InvalidInput, "Package name is required.");
            }
          
            Packages packages = new Packages()
            {
                Name = packageVM.Name,
                Price = packageVM.Price,
                Image = packageVM.Image,
                Information = packageVM.Information,
                Experiences = packageVM.Experiences,
                CreatedTime = DateTime.Now,
            };
            await _unitOfWork.GetRepository<Packages>().InsertAsync(packages);
            await _unitOfWork.SaveAsync();
        }
        public async Task Delete(string packageID)
        {
            Packages? existedPackage = await _unitOfWork.GetRepository<Packages>().GetByIdAsync(packageID);
            if (existedPackage==null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Not found Package");
            }
            existedPackage.DeletedTime = DateTime.Now;
            //existedPackage.DeletedBy = ehehehheh;
            await _unitOfWork.GetRepository<Packages>().UpdateAsync(existedPackage);
            await _unitOfWork.SaveAsync();
        }

        public async Task<BasePaginatedList<GETPackageViewModel>> GetAll(int pageNumber = 1, int pageSize = 2)
        {
            var packages = await _unitOfWork.GetRepository<Packages>().GetAllAsync();
           
            var packageViewModels = packages.Select(pa => new GETPackageViewModel
            {
                Id = pa.Id,
                Name=pa.Name,
                Price=pa.Price,
                Image=pa.Image,
                Information=pa.Information,
                Experiences=pa.Experiences,
                //THiếu user => chưa làm createby,deleteby,updateby
                //CreatedBy=pa.CreatedBy,
                //LastUpdatedBy=pa.LastUpdatedBy,
                //DeletedBy=pa.DeletedBy,
                ServiceEntityResponseModels = pa.Service.Select(se => new ServiceEntityResponseModel
                {
                    Id = se.Id.ToString(),
                    Name = se.Name,
                }).ToList()

            }).ToList();

            //Count Package
            int totalPackage = packages.Count;

            var paginatedPackages = packageViewModels
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new BasePaginatedList<GETPackageViewModel>(paginatedPackages, totalPackage, pageNumber, pageSize);
        }

<<<<<<< HEAD
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
=======
        public async Task<GETPackageViewModel?> GetById(string packageID)
        {
            // Kiểm tra xem packageID có hợp lệ không
            if (string.IsNullOrWhiteSpace(packageID))
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Invalid package ID.");
            }
            IQueryable<Packages> query = _unitOfWork.GetRepository<Packages>().Entities.Where(q => !q.DeletedTime.HasValue);
            var existedPackage = await _unitOfWork.GetRepository<Packages>().Entities.FirstOrDefaultAsync(p => p.Id == packageID);
            if (existedPackage != null)
            {
                return new GETPackageViewModel
                {
                    Id = existedPackage.Id,
                    Name = existedPackage.Name,
                    Price = existedPackage.Price,
                    Image = existedPackage.Image,
                    Information = existedPackage.Information,
                    Experiences = existedPackage.Experiences,
                    //THiếu user => chưa làm createby,deleteby,updateby
                    //CreatedBy=pa.CreatedBy,
                    //LastUpdatedBy=pa.LastUpdatedBy,
                    //DeletedBy=pa.DeletedBy,
                    ServiceEntityResponseModels = existedPackage.Service.Select(se => new ServiceEntityResponseModel
                    {
                        Id = se.Id.ToString(),
                        Name = se.Name,
                    }).ToList()
                };
               
            }
            throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Not found Package");

        }

        public async Task<List<GETPackageViewModel?>> GetPackages(string packageID, DateTime? DateStart, DateTime? EndStart, string? Name)
        {
            //IQueryable<Packages> query = _unitOfWork.GetRepository<Packages>().Entities.Where(q => !q.DeletedTime.HasValue);

            //// Lọc theo packageID nếu có
            //if (!string.IsNullOrWhiteSpace(packageID))
            //{
            //    query = query.Where(q => q.Equals(packageID));
            //}

            //// Lọc theo DateStart và EndStart nếu có
            //if (DateStart!=null)
            //{
            //    query = query.Where(q => q.CreatedTime >= DateStart.Value);
            //}
            //if (EndStart!=null)
            //{
            //    query = query.Where(q => q.CreatedTime <= EndStart.Value);
            //}

            //// Lọc theo Name nếu có
            //if (!string.IsNullOrWhiteSpace(Name))
            //{
            //    query = query.Where(q => q.Name.Contains(Name));
            //}

            //// Chuyển đổi dữ liệu sang GETPackageViewModel
            //return await query
            //    .OrderByDescending(pa => pa.CreatedTime!=null)
            //    .Select(pa => new GETPackageViewModel
            //    {
            //        Id = packageID, // Sử dụng PackageID từ cơ sở dữ liệu thay vì packageID từ tham số
            //        Name = pa.Name,
            //        Price = pa.Price,
            //        Image = pa.Image,
            //        Information = pa.Information,
            //        Experiences = pa.Experiences,

            //        // Chuyển đổi các Service thành ServiceEntityResponseModel
            //        ServiceEntityResponseModels = pa.Service.Select(se => new ServiceEntityResponseModel
            //        {
            //            Id = se.Id.ToString(),
            //            Name = se.Name,
            //        }).ToList() ?? new List<ServiceEntityResponseModel>()
            //    })
            //    .ToListAsync();
            throw new NotImplementedException();
>>>>>>> 05e26ddaeb54be6bb24dbaadf87ff8883bb5426d
        }


        public async Task Update(Packages package)
        {
            IGenericRepository<Packages> genericRepository = _unitOfWork.GetRepository<Packages>();
            await genericRepository.UpdateAsync(package);
            await _unitOfWork.SaveAsync();
        }

     
    }
}
