using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Repositories.IUOW;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.Core.Infrastructure;
using PetSpa.Core.Utils;
using PetSpa.ModelViews.PackageModelViews;
using PetSpa.Repositories.UOW;

using ServicesEntity = PetSpa.Contract.Repositories.Entity.Services;

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
            if (packageMV.ServiceIDs == null || packageMV.ServiceIDs.Count == 0)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Package must include at least one service.");
            }
            // Kiểm tra trùng lặp trong danh sách ServiceIDs
            var duplicateServiceIds = packageMV.ServiceIDs
                                               .GroupBy(id => id)
                                               .Where(g => g.Count() > 1)
                                               .Select(g => g.Key)
                                               .ToList();

            if (duplicateServiceIds.Any())
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput,
                    $"Duplicate service IDs are not allowed: {string.Join(", ", duplicateServiceIds)}");
            }
            var services = await _unitOfWork.GetRepository<ServicesEntity>()
                             .Entities
                             .Where(p => packageMV.ServiceIDs.Contains(p.Id))
                             .ToListAsync();
            if (services.Count != packageMV.ServiceIDs.Count)
            {
                // Tìm các ID không hợp lệ
                var invalidServiceIds = packageMV.ServiceIDs.Except(services.Select(s => s.Id)).ToList();
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput,
                    $"The following service IDs are invalid: {string.Join(", ", invalidServiceIds)}");
            }
            Packages packages = new Packages()
            {
                Name = packageMV.Name,
                Price = packageMV.Price,
                Image = packageMV.Image,
                Information = packageMV.Information,
                Experiences = packageMV.Experiences,
                CreatedTime = TimeHelper.ConvertToUtcPlus7(DateTime.Now),
                CreatedBy=currentUserId
            };
            await _unitOfWork.GetRepository<Packages>().InsertAsync(packages);
            await _unitOfWork.SaveAsync();
            // Thêm mối quan hệ giữa OrderDetail và Packages
            foreach (var service in services)
            {
                var packageServiceDTO = new PackageServiceEntity
                {
                    ServicesEntityID = service.Id,
                    PackageId = packages.Id,
                };
                await _unitOfWork.GetRepository<PackageServiceEntity>().InsertAsync(packageServiceDTO);
            }
            await _unitOfWork.SaveAsync();
        }
        public async Task Delete(string packageID)
        {
            
            Packages? existedPackage = await _unitOfWork.GetRepository<Packages>().GetByIdAsync(packageID);
            if (existedPackage == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Not found Package");
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
            // Kiểm tra nếu packageMV null
            if (packageMV == null)
            {
                throw new BadRequestException(ErrorCode.BadRequest, "Package cannot be null.");
            }

            // Kiểm tra nếu Id bị thiếu hoặc không hợp lệ
            if (packageMV.Id == null)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Package id is required.");
            }

            // Kiểm tra sự tồn tại của Package dựa trên Id
            Packages? existedPackage = await _unitOfWork.GetRepository<Packages>().GetByIdAsync(packageMV.Id);
            if (existedPackage == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Package not found.");
            }

            // Kiểm tra các thuộc tính còn lại
            if (packageMV.Name == null)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Package name is required.");
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