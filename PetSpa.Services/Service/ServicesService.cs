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
using AutoMapper;
namespace PetSpa.Services.Service
{
    public class ServicesService : IServicesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMapper _mapper;

        private string currentUserId => Authentication.GetUserIdFromHttpContextAccessor(_contextAccessor);
        public ServicesService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _mapper = mapper;
        }
        public async Task Add(ServiceCreateModel serviceModel)
        {
            if (string.IsNullOrWhiteSpace(serviceModel.Name))
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Service name cannot be null or empty.");
            }
            ServicesEntity service = _mapper.Map<ServicesEntity>(serviceModel);
            service.CreatedBy = currentUserId;
            await _unitOfWork.GetRepository<ServicesEntity>().InsertAsync(service);
            await _unitOfWork.SaveAsync();
        }

        public async Task Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Invalid service ID.");
            }
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
            if (pageNumber <= 0 || pageSize <= 0)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Pagenumber and pagesize must greater than 0");
            }
            // Retrieve the repository for ServicesEntity
            var genericRepository = _unitOfWork.GetRepository<ServicesEntity>();

            // Create a query for the services that are not marked as deleted
            IQueryable<ServicesEntity> servicesQuery = genericRepository.Entities
                .Where(s => s.DeletedTime.HasValue == false); // Filter out soft-deleted records

            // Use the pagination method to get the paginated list of services
            BasePaginatedList<ServicesEntity> paginatedServices = await genericRepository.GetPagging(servicesQuery, pageNumber, pageSize);

            // Map the ServicesEntity to ServiceResponseModel
            var serviceResponseModels = paginatedServices.Items.Select(s => _mapper.Map<ServiceResposeModel>(s)).ToList();

            // Return the paginated list of service response models
            return new BasePaginatedList<ServiceResposeModel>(serviceResponseModels, paginatedServices.TotalItems, pageNumber, pageSize);
        }



        public async Task<ServiceResposeModel?> GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Invalid service ID.");
            }
            var service = await _unitOfWork.GetRepository<ServicesEntity>().Entities.FirstOrDefaultAsync(s => s.DeletedTime.HasValue == false && s.Id ==  id);
            if (service == null) {
                throw new ErrorException(statusCode: StatusCodes.Status404NotFound, errorCode: ErrorCode.NotFound, "Not found Service with id =" + id);
            }
            ServiceResposeModel serviceRespose = _mapper.Map<ServiceResposeModel>(service);
            return serviceRespose;
        }


        public async Task Update(ServiceUpdateModel serviceModel)
        {
            if (string.IsNullOrWhiteSpace(serviceModel.Name))
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Service name cannot be null or empty.");
            }
            IGenericRepository<ServicesEntity> genericRepository = _unitOfWork.GetRepository<ServicesEntity>();
            
            ServicesEntity? service = await _unitOfWork.GetRepository<ServicesEntity>().Entities.FirstOrDefaultAsync(s => s.DeletedTime.HasValue == false && s.Id == serviceModel.Id);
            if (service == null)
            {
                throw new ErrorException(statusCode: StatusCodes.Status404NotFound, errorCode: ErrorCode.NotFound, "Not found Service with id =" + serviceModel.Id);
            }

            _mapper.Map(serviceModel, service);
            service.LastUpdatedBy = currentUserId;
           
            await genericRepository.UpdateAsync(service);
            await _unitOfWork.SaveAsync();
        }
    }
}
