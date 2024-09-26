
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Repositories.IUOW;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.Core.Utils;
using PetSpa.ModelViews.ModelViews;
using PetSpa.ModelViews.PackageModelViews;
using PetSpa.ModelViews.PackageServiceModelViews;
using PetSpa.ModelViews.ServiceModelViews;
using PetSpa.Repositories.UOW;

namespace PetSpa.Services.Service
{
    public class PackageService : IPackageService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PackageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task Add(POSTPackageModelView packageMV)
        {
            if (packageMV == null)
            {
                throw new BadRequestException(ErrorCode.BadRequest, "Package cannot null.");
            }
            if (packageMV.Name == null)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Package name is required.");
            }
            if (packageMV.Price < 0)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Price must be greater than or equal to 0.");
            }

            Packages packages = new Packages()
            {
                Name = packageMV.Name,
                Price = packageMV.Price,
                Image = packageMV.Image,
                Information = packageMV.Information,
                Experiences = packageMV.Experiences,
                CreatedTime = DateTime.Now,
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
            existedPackage.DeletedTime = DateTime.Now;
            //existedPackage.DeletedBy = ehehehheh;
            await _unitOfWork.GetRepository<Packages>().DeleteAsync(packageID);
            await _unitOfWork.SaveAsync();
        }

        public async Task<BasePaginatedList<GETPackageModelView>> GetAll(int pageNumber = 1, int pageSize = 2)
        {


            var packages = await _unitOfWork.GetRepository<Packages>()
                                .Entities
                                .Include(p => p.PackageServices!)
                                .ThenInclude(ps => ps.ServicesEntity)
                                .ToListAsync();
            if (packages == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Not found Package");
            }

            var packageViewModels = packages.Select(pa => new GETPackageModelView
            {
                Id = pa.Id,
                Name = pa.Name,
                Price = pa.Price,
                Image = pa.Image,
                Information = pa.Information,
                Experiences = pa.Experiences,
                CreatedTime = pa.CreatedTime,

                listService = pa.PackageServices?.Select(s => new GETPackageServiceModelView
                {
                    ServiceId = s.ServicesEntity?.Id,
                    ServiceName = s.ServicesEntity?.Name,
                }).ToList(),


            }).ToList();

            int totalPackage = packages.Count;

            var paginatedPackages = packageViewModels
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new BasePaginatedList<GETPackageModelView>(paginatedPackages, totalPackage, pageNumber, pageSize);
        }


        public async Task<GETPackageModelView?> GetById(string? packageID)
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
            return new GETPackageModelView
            {
                Id = existedPackage.Id,
                Name = existedPackage.Name,
                Price = existedPackage.Price,
                Image = existedPackage.Image,
                Information = existedPackage.Information,
                Experiences = existedPackage.Experiences,
                CreatedTime = existedPackage.CreatedTime
                //THiếu user => chưa làm createby,deleteby,updateby
                //CreatedBy=pa.CreatedBy,
                //LastUpdatedBy=pa.LastUpdatedBy,
                //DeletedBy=pa.DeletedBy,

            };

        }

        public async Task<List<GETPackageModelView?>> GetPackages(string packageID, DateTime? DateStart, DateTime? EndStart, string? Name)
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
        }
        public async Task Update(PUTPackageModelView packageMV)
        {
            Packages? existedPackage = await _unitOfWork.GetRepository<Packages>().GetByIdAsync(packageMV.Id);
            if (existedPackage == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Not found Package");
            }
            if (packageMV == null)
            {
                throw new BadRequestException(ErrorCode.BadRequest, "Package cannot null.");
            }
            if (packageMV.Name == null)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Package name is required.");
            }
            if (packageMV.Price < 0)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Price must be greater than or equal to 0.");
            }
            existedPackage.Name = packageMV.Name;
            existedPackage.Price = packageMV.Price;
            existedPackage.Image = packageMV.Image;
            existedPackage.Information = packageMV.Information;
            existedPackage.Experiences = packageMV.Experiences;
            existedPackage.LastUpdatedTime = DateTime.Now;
            await _unitOfWork.GetRepository<Packages>().UpdateAsync(existedPackage);
            await _unitOfWork.SaveAsync();
        }


    }
}
