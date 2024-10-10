using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Repositories.IUOW;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.Core.Infrastructure;
using PetSpa.Core.Utils;
using PetSpa.ModelViews.PackageModelViews;
using PetSpa.ModelViews.PackageServiceModelViews;
using PetSpa.Repositories.UOW;
namespace PetSpa.Services.Service
{
    public class PackageService : IPackageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private string currentUserId => Authentication.GetUserIdFromHttpContextAccessor(_contextAccessor);

        public PackageService(IUnitOfWork unitOfWork,IHttpContextAccessor contextAccessor)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
        }
        public async Task Add(POSTPackageModelView packageMV)
        {
            if (packageMV.Price < 0)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Price must be greater than or equal to 0.");
            }
            var user = await _unitOfWork.GetRepository<ApplicationUser>().GetByIdAsync(Guid.Parse(currentUserId));
            Packages packages = new Packages()
            {
                Name = packageMV.Name,
                Price = packageMV.Price,
                Image = packageMV.Image,
                Information = packageMV.Information,
                Experiences = packageMV.Experiences,
                CreatedTime = TimeHelper.ConvertToUtcPlus7(DateTime.Now),
                CreatedBy=user?.UserName
            };
            await _unitOfWork.GetRepository<Packages>().InsertAsync(packages);
            await _unitOfWork.SaveAsync();
        }
        public async Task Delete(string packageID)
        {
            
            Packages? existedPackage = await _unitOfWork.GetRepository<Packages>().GetByIdAsync(packageID);
            if (existedPackage == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Not found Package");
            }
            var relatedServices =  _unitOfWork.GetRepository<PackageServiceEntity>().Entities.Where(p => p.PackageId == packageID).ToList();

            foreach (var service in relatedServices)
            {
                await _unitOfWork.GetRepository<PackageServiceEntity>().DeleteAsync(service.Id); // Assuming Id is the primary key
                await _unitOfWork.SaveAsync();
            }

            //existedPackage.DeletedTime = TimeHelper.ConvertToUtcPlus7(DateTime.Now);
            ////existedPackage.DeletedBy = ehehehheh;
            await _unitOfWork.GetRepository<Packages>().DeleteAsync(packageID);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteServiceInPakcage(string serviceINPackageID)
        {
            PackageServiceEntity? existedServiceINPackage = await _unitOfWork.GetRepository<PackageServiceEntity>().GetByIdAsync(serviceINPackageID);
            if (existedServiceINPackage == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Not found Service");
            }
          
            await _unitOfWork.GetRepository<PackageServiceEntity>().DeleteAsync(serviceINPackageID);
            await _unitOfWork.SaveAsync();
        }

        public async Task<BasePaginatedList<GETPackageModelView>> GetAll(int pageNumber , int pageSize)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Pagenumber and pagesize must greater than 0");
            }
            IQueryable<Packages> packages = _unitOfWork.GetRepository<Packages>()
               .Entities.Where(i => !i.DeletedTime.HasValue)//Membership chưa bị xóa
               .OrderByDescending(c => c.CreatedTime).AsQueryable();// Sắp xếp theo thời gian tạo
            //Phân trang và chỉ lấy các bản ghi cần thiết
            var paginatedPackages = await packages
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(pa => new GETPackageModelView
                {
                    Id = pa.Id,
                    Name = pa.Name,
                    Price = pa.Price,
                    Image = pa.Image,
                    Information = pa.Information,
                    Experiences = pa.Experiences,
                    CreatedTime = pa.CreatedTime,
                    CreatedBy = pa.CreatedBy,
                }).ToListAsync();
            return new BasePaginatedList<GETPackageModelView>(paginatedPackages, await packages.CountAsync(), pageNumber, pageSize);
        }
        public async Task AddServiceInPackage(string packageID, string serviceID)
        {
            if (string.IsNullOrWhiteSpace(packageID))
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Invalid package ID.");
            }

            var existedPackage = await _unitOfWork.GetRepository<Packages>().Entities.FirstOrDefaultAsync(p => p.Id == packageID);
            if (existedPackage == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Not found Package");
            }

            var existedService = await _unitOfWork.GetRepository<ServicesEntity>().Entities.FirstOrDefaultAsync(p => p.Id == serviceID);
            if (existedService == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Not found Service");
            }

            // Kiểm tra xem dịch vụ đã tồn tại trong gói chưa
            var packageServiceExists = await _unitOfWork.GetRepository<PackageServiceEntity>()
                .Entities.AnyAsync(ps => ps.PackageId == packageID && ps.ServicesEntityID == serviceID);

            if (packageServiceExists)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Service already exists in the package.");
            }

            PackageServiceEntity packageService = new PackageServiceEntity()
            {
                PackageId = packageID,
                ServicesEntityID = serviceID,
                CreatedTime = DateTime.Now,
            };

            await _unitOfWork.GetRepository<PackageServiceEntity>().InsertAsync(packageService);
            await _unitOfWork.SaveAsync();
        }

        public async Task<GETPackageModelView?> GetById(string? packageID)
        {
            if (string.IsNullOrWhiteSpace(packageID))
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Invalid package ID.");
            }
            var existedPackage = await _unitOfWork.GetRepository<Packages>()
                              .Entities
                              .FirstOrDefaultAsync(p => p.Id == packageID);
            if (existedPackage == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Not found Package");
            }
            return new GETPackageModelView
            {
                Id = existedPackage.Id,
                Name = existedPackage.Name,
                Price = existedPackage.Price,
                Image = existedPackage.Image,
                Information = existedPackage.Information,
                Experiences = existedPackage.Experiences,
                CreatedTime = existedPackage.CreatedTime,
            };
        }
        public async Task<List<GETPackageServiceModelView>> GetServicesByPackageId(string packageId)
        {
            if (string.IsNullOrWhiteSpace(packageId))
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Invalid package ID.");
            }

            var existedPackage = await _unitOfWork.GetRepository<Packages>()
                         .Entities
                         .Include(p => p.PackageServices!) // Bao gồm Services liên kết với Package
                         .ThenInclude(ps => ps.ServicesEntity) // Bao gồm Service liên kết
                         .FirstOrDefaultAsync(p => p.Id == packageId);
            if (existedPackage == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Package not found.");
            }
            // Lấy danh sách service từ PackageServices
            var serviceList = existedPackage.PackageServices?
                                .Select(ps => new GETPackageServiceModelView
                                {
                                    Id = ps.Id,
                                    ServiceName = ps.ServicesEntity?.Name ?? string.Empty
                                }).ToList();

            return serviceList ?? new List<GETPackageServiceModelView>();
        }

        public async Task<List<GETPackageModelView>?> GetPackageByConditions(DateTimeOffset? DateStart, DateTimeOffset? DateEnd)
        {
            // Khởi tạo truy vấn cho bảng Packages
            IQueryable<Packages> query = _unitOfWork.GetRepository<Packages>()
                .Entities.Where(q => !q.DeletedTime.HasValue); // Chỉ lấy những gói chưa bị xóa
            // Lọc theo DateStart nếu có
            if (DateStart.HasValue)
            {
                query = query.Where(p => p.CreatedTime >= DateStart.Value);
            }
            // Lọc theo DateEnd nếu có
            if (DateEnd.HasValue)
            {
                query = query.Where(p => p.CreatedTime <= DateEnd.Value);
            }

            // Lấy dữ liệu từ cơ sở dữ liệu
            var packages = await query.ToListAsync();

            // Kiểm tra nếu không tìm thấy gói nào
            if (!packages.Any())
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "No packages found.");
            }

            // Chuyển đổi dữ liệu sang GETPackageModelView
            var packageViewModels = packages.Select(p => new GETPackageModelView
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Image = p.Image,
                Information = p.Information,
                Experiences = p.Experiences,
                CreatedTime = p.CreatedTime,
                CreatedBy = p.CreatedBy,
            }).ToList();
            return packageViewModels;
        }

        public async Task Update(PUTPackageModelView packageMV)
        {
            if (string.IsNullOrWhiteSpace(packageMV.Id))
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Package cannot null or whitespace");
            }

            // Kiểm tra sự tồn tại của Package dựa trên Id
            Packages? existedPackage = await _unitOfWork.GetRepository<Packages>().GetByIdAsync(packageMV.Id);
            if (existedPackage == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Package not found.");
            }
            if (packageMV.Price < 0)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Price must be greater than or equal to 0.");
            }

            // Cập nhật thông tin của Package
            existedPackage.Name = packageMV.Name;
            existedPackage.Price = packageMV.Price;
            existedPackage.Image = packageMV.Image;
            existedPackage.Information = packageMV.Information;
            existedPackage.Experiences = packageMV.Experiences;
            existedPackage.LastUpdatedTime = TimeHelper.ConvertToUtcPlus7(DateTime.Now);
            existedPackage.LastUpdatedBy = currentUserId;
            // Thực hiện cập nhật trong cơ sở dữ liệu
            await _unitOfWork.GetRepository<Packages>().UpdateAsync(existedPackage);
            await _unitOfWork.SaveAsync();      
        }

    }
}