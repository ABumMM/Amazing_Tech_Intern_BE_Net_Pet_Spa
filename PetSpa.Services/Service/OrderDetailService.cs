using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Repositories.IUOW;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.ModelViews.OrderDetailModelViews;
using PetSpa.ModelViews.PackageModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.Services.Service
{
    public class OrderDetailService : IOrderDetailServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderDetailService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Add(POSTOrderDetailModelView detailMV)
        {
            if (detailMV == null)
            {
                throw new BadRequestException(ErrorCode.BadRequest, "OrderDetail cannot null.");
            }
            if (detailMV.Status == null)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "OrderDetail Status is required.");
            }
            if (detailMV.Price < 0)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Price must be greater than or equal to 0.");
            } 
            if (detailMV.Quantity < 0)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Quantity must be greater than or equal to 0.");
            }

            OrdersDetails details = new OrdersDetails()
            {
                Quantity = (int)detailMV.Quantity,
                Price = (decimal)detailMV.Price,
                Status = detailMV.Status,
                CreatedTime = DateTime.Now,
            };
            await _unitOfWork.GetRepository<OrdersDetails>().InsertAsync(details);
            await _unitOfWork.SaveAsync();
        }

        public async Task Delete(string OrDetailID)
        {
            OrdersDetails? existedOrDetail = await _unitOfWork.GetRepository<OrdersDetails>().GetByIdAsync(OrDetailID);
            if (existedOrDetail == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Not found OrderDetail");
            }
            existedOrDetail.DeletedTime = DateTime.Now;
            await _unitOfWork.GetRepository<OrdersDetails>().DeleteAsync(OrDetailID);
            await _unitOfWork.SaveAsync();
        }

        public async Task<BasePaginatedList<GETOrderDetailModelView>> getAll(int pageNumber = 1, int pageSize = 3)
        {
            var orDetails = await _unitOfWork.GetRepository<OrdersDetails>().GetAllAsync();

            var orDViewModels = orDetails.Select(orD => new GETOrderDetailModelView
            {
                Id = orD.Id,
                Quantity = orD.Quantity,
                Price = orD.Price,
                Status = orD.Status,
                CreatedTime = orD.CreatedTime,
            }).ToList();

            //Count OrderDetail
            int totalOrDetail = orDetails.Count;

            var paginatedOrDetail = orDViewModels
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new BasePaginatedList<GETOrderDetailModelView>(paginatedOrDetail, totalOrDetail, pageNumber, pageSize);

        }

        public async Task<GETOrderDetailModelView?> getById(string OrDetailID)
        {
            if (string.IsNullOrWhiteSpace(OrDetailID))
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Invalid OrderDetail ID.");
            }
            var existedOrDetail = await _unitOfWork.GetRepository<OrdersDetails>().Entities.FirstOrDefaultAsync(orD => orD.Id == OrDetailID);
            if (existedOrDetail == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Not found OrderDetail");
            }

            return new GETOrderDetailModelView
            {
                Id = existedOrDetail.Id,
                Quantity = existedOrDetail.Quantity,
                Status = existedOrDetail.Status,
                Price = existedOrDetail.Price
            };
        }

        public async Task Update(PUTOrderDetailModelView detailsMV)
        {
            OrdersDetails? existedOrDetail = await _unitOfWork.GetRepository<OrdersDetails>().GetByIdAsync(detailsMV.Id);

            if (existedOrDetail == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Not found OrderDetail");
            }
            if (detailsMV == null)
            {
                throw new BadRequestException(ErrorCode.BadRequest, "OrderDetail cannot null.");
            }
            if (detailsMV.Status == null)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "OrderDetail status is required.");
            }
            if (detailsMV.Price < 0)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Price must be greater than or equal to 0.");
            } 
            if (detailsMV.Quantity < 0)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Quantity must be greater than or equal to 0.");
            }
            existedOrDetail.Quantity = (int)detailsMV.Quantity;
            existedOrDetail.Status = detailsMV.Status;
            existedOrDetail.Price = (decimal)detailsMV.Price;
            existedOrDetail.LastUpdatedTime = DateTime.Now;

            await _unitOfWork.GetRepository<OrdersDetails>().UpdateAsync(existedOrDetail);
            await _unitOfWork.SaveAsync();

        }
    }
}
