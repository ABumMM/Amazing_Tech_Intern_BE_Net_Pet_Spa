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
using ServicesEntity = PetSpa.Contract.Repositories.Entity.Services;
using Microsoft.EntityFrameworkCore;
using PetSpa.Repositories.UOW;
using PetSpa.Core.Infrastructure;
namespace PetSpa.Services.Service
{
    public class ServicesService : IServicesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private string currentUserId => Authentication.GetUserIdFromHttpContextAccessor(_contextAccessor);
        public ServicesService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
        }
        public async Task Add(ServiceCreateModel serviceModel)
        {
            ServicesEntity service = new ServicesEntity
            {
                Name = serviceModel.Name,
                Description = serviceModel.Description,
                CreatedBy = currentUserId
            }; 
            await _unitOfWork.GetRepository<ServicesEntity>().InsertAsync(service);
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
            service.DeletedBy = currentUserId;
            service.DeletedTime = DateTime.UtcNow;
            await _unitOfWork.SaveAsync();
        }

        public async Task<BasePaginatedList<ServiceResposeModel>> GetAll(int pageNumber, int pageSize)
        {
            // Retrieve the repository for ServicesEntity
            var genericRepository = _unitOfWork.GetRepository<ServicesEntity>();

            // Create a query for the services that are not marked as deleted
            IQueryable<ServicesEntity> servicesQuery = genericRepository.Entities
                .Where(s => s.DeletedTime.HasValue == false); // Filter out soft-deleted records

            // Use the pagination method to get the paginated list of services
            BasePaginatedList<ServicesEntity> paginatedServices = await genericRepository.GetPagging(servicesQuery, pageNumber, pageSize);

            // Map the ServicesEntity to ServiceResponseModel
            var serviceResponseModels = paginatedServices.Items.Select(s => new ServiceResposeModel
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description
            }).ToList();

            // Return the paginated list of service response models
            return new BasePaginatedList<ServiceResposeModel>(serviceResponseModels, paginatedServices.TotalItems, pageNumber, pageSize);
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
            service.LastUpdatedBy = currentUserId;
            service.LastUpdatedTime = DateTime.Now;
            
           
            await genericRepository.UpdateAsync(service);
            await _unitOfWork.SaveAsync();
        }
    }
}
