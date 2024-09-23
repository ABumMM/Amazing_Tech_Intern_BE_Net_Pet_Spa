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
            PackageServiceDTO packageService = new PackageServiceDTO()
            {
                PackageId = packageID,
                ServiceId = serviceID,
                AddedBy =null,
                AddedDate = DateTime.Now,
            };
            await _unitOfWork.GetRepository<PackageServiceDTO>().InsertAsync(packageService);
            await _unitOfWork.SaveAsync();
        }

        public async Task Delete(string packageID, string serviceID)
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
            PackageServiceDTO? existedPackageServiceDTO = await _unitOfWork.GetRepository<PackageServiceDTO>().GetByIdAsync(packageID);
            if (existedPackageServiceDTO == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Not found Package");
            }
          
            await _unitOfWork.GetRepository<Packages>().DeleteAsync(packageID);
            await _unitOfWork.SaveAsync();
        }

        public Task<BasePaginatedList<GETPackageServiceModelView>> GetAll(int pageNumber = 1, int pageSize = 3)
        {
            throw new NotImplementedException();
        }

        public Task<GETPackageServiceModelView?> GetById(string packageID, string serviceID)
        {
            throw new NotImplementedException();
        }

        public Task<List<GETPackageModelView?>> GetPackages(string packageID, DateTime? DateStart, DateTime? EndStart, string? Name)
        {
            throw new NotImplementedException();
        }
    }
}
