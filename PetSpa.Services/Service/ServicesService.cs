using PetSpa.Contract.Repositories.Entity;
﻿using Microsoft.AspNetCore.Http;
using PetSpa.Contract.Repositories.IUOW;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.ModelViews.ServiceModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.Services.Service
{
    public class ServicesService : IServicesService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServicesService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task Add(ServiceCreateModel serviceModel)
        {


            ServicesEntity service = new ServicesEntity
            {
                Id = Guid.NewGuid().ToString("N"),
                Name = serviceModel.Name,
                Description = serviceModel.Description,
            };

            IGenericRepository<ServicesEntity> genericRepository = _unitOfWork.GetRepository<ServicesEntity>();
            await genericRepository.InsertAsync(service);
            await _unitOfWork.SaveAsync();
        }

        public async Task Delete(string id)
        {
            IGenericRepository<ServicesEntity> genericRepository = _unitOfWork.GetRepository<ServicesEntity>();
            ServicesEntity? service = genericRepository.GetById(id);
            if (service == null)
            {
                throw new ErrorException(statusCode: StatusCodes.Status404NotFound, errorCode: ErrorCode.NotFound, "Not found Service with id =" + id);
            }
           
            await genericRepository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }

        public async Task<BasePaginatedList<ServiceResposeModel>> GetAll(int pageNumber = 1, int pageSize = 10)
        {
            var service_lst = await _unitOfWork.GetRepository<ServicesEntity>().GetAllAsync();

            // Map to ServiceResponseModel
            var serviceResponseModels = service_lst.Select(s => new ServiceResposeModel
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                Price = s.Price
                //PackageId = s.PackageId,
            }).ToList();

            // Calculate total number of items
            var totalServices = serviceResponseModels.Count;

            // Paginate the results
            var paginatedServices = serviceResponseModels
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Create the paginated list
            return new BasePaginatedList<ServiceResposeModel>(paginatedServices, totalServices, pageNumber, pageSize);
        }



        public async Task<ServiceResposeModel?> GetById(object id)
        {
            var service = await _unitOfWork.GetRepository<ServicesEntity>().GetByIdAsync(id);
            if (service == null) {
                throw new ErrorException(statusCode: StatusCodes.Status404NotFound, errorCode: ErrorCode.NotFound, "Not found Service with id =" + id);
            }
            ServiceResposeModel serviceRespose = new ServiceResposeModel
            {
                Id = service.Id,
                Name = service.Name,
                Description = service.Description,
                Price = service.Price
            };
            return serviceRespose;
        }


        public async Task Update(ServiceUpdateModel serviceModel)
        {
            IGenericRepository<ServicesEntity> genericRepository = _unitOfWork.GetRepository<ServicesEntity>();
            ServicesEntity? service = genericRepository.GetById(serviceModel.Id);
            if (service == null)
            {
                throw new ErrorException(statusCode: StatusCodes.Status404NotFound, errorCode: ErrorCode.NotFound, "Not found Service with id =" + serviceModel.Id);
            }

            service.Name = serviceModel.Name;
            service.Description = serviceModel.Description;
            service.Price = serviceModel.Price;
            service.LastUpdatedTime = DateTime.Now;
            
           
            await genericRepository.UpdateAsync(service);
            await _unitOfWork.SaveAsync();
        }
    }
}
