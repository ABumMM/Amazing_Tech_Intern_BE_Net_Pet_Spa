using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Repositories.IUOW;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.ModelViews.PackageModelViews;
using PetSpa.ModelViews.PackageServiceModelViews;
using PetSpa.Repositories.UOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.Services.Service
{
    public class PackageService_Service : IPackageService_Service
    {
        private readonly IUnitOfWork _unitOfWork;

        public PackageService_Service(IUnitOfWork unitOfWork) 
        {
            _unitOfWork=unitOfWork;
        }

        public async Task Add(string packageID, string serviceID)
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
            var packageServiceExists = await _unitOfWork.GetRepository<PackageServiceDTO>()
                .Entities.AnyAsync(ps => ps.PackageId == packageID && ps.ServicesEntityID == serviceID);

            if (packageServiceExists)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Service already exists in the package.");
            }

            PackageServiceDTO packageService = new PackageServiceDTO()
            {
                PackageId = packageID,
                ServicesEntityID = serviceID,
                CreatedTime = DateTime.Now,
            };

            await _unitOfWork.GetRepository<PackageServiceDTO>().InsertAsync(packageService);
            await _unitOfWork.SaveAsync();
        }


        public async Task Delete(string ID)
        {
            if (string.IsNullOrWhiteSpace(ID))
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Invalid package ID.");
            }
            //Kiểm tra đã tồn tại 
            var existed = await _unitOfWork.GetRepository<PackageServiceDTO>().GetByIdAsync(ID);
            if (existed == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Not found ");
            }
            await _unitOfWork.GetRepository<PackageServiceDTO>().DeleteAsync(ID);
            await _unitOfWork.SaveAsync();
        }

        public async Task<BasePaginatedList<GETPackageServiceModelView>> GetAll(int pageNumber,int pageSize)
        {
            var packageservice = await _unitOfWork.GetRepository<PackageServiceDTO>().GetAllAsync();

            var ViewModels = packageservice.Select(pa => new GETPackageServiceModelView
            {
                Id = pa.Id,
                PackageId=pa.PackageId,
                ServiceId=pa.ServicesEntityID,
                CreatedTime = DateTime.Now,
            }).ToList();

            int totalPackage = packageservice.Count;

            var paginatedList = ViewModels
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new BasePaginatedList<GETPackageServiceModelView>(paginatedList, totalPackage, pageNumber, pageSize);
        }

        public async Task<GETPackageServiceModelView?> GetById(string ID)
        {
            if (string.IsNullOrWhiteSpace(ID))
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Invalid ID.");
            }
            //Kiểm tra đã tồn tại 
            var existed = await _unitOfWork.GetRepository<PackageServiceDTO>().GetByIdAsync(ID);
            if (existed == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Not found ");
            }
            existed.DeletedTime = DateTime.Now;
         
            await _unitOfWork.GetRepository<PackageServiceDTO>().DeleteAsync(ID);
            await _unitOfWork.SaveAsync();
            return new GETPackageServiceModelView
            {
                Id = existed.Id,
                PackageId = existed.PackageId,
                ServiceId = existed.ServicesEntityID

            };
        }

        public Task<List<GETPackageModelView?>> GetPackages(string packageID, DateTime? DateStart, DateTime? EndStart, string? Name)
        {
            throw new NotImplementedException();
        }
    }
}
