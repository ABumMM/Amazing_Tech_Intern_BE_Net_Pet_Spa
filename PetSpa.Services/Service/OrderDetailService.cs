using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Repositories.IUOW;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.ModelViews.OrderDetailModelViews;
using PetSpa.ModelViews.PackageModelViews;
using System.Security.Cryptography;

namespace PetSpa.Services.Service
{
    public class OrderDetailService : IOrderDetailServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderDetailService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Add(POSTOrderDetailModelView detailsMV)
        {
            if (detailsMV == null)
            {
                throw new BadRequestException(ErrorCode.BadRequest, "OrderDetail cannot null.");
            }
            if (detailsMV.Status == null)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "OrderDetail Status is required.");
            }
            if (detailsMV.Price < 0)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Price must be greater than or equal to 0.");
            } 
            if (detailsMV.Quantity < 0)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Quantity must be greater than or equal to 0.");
            }

            // Kiểm tra từng PackageID
            var packages = await _unitOfWork.GetRepository<Packages>()
                                              .Entities
                                              .Where(p => detailsMV.PackageIDs.Contains(p.Id))
                                              .ToListAsync();
            if (packages.Count != detailsMV.PackageIDs.Count)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "One or more PackageIDs not found.");
            }

            OrdersDetails details = new OrdersDetails()
            {
                Quantity = (int)detailsMV.Quantity,
                Price = (decimal)detailsMV.Price,
                Status = detailsMV.Status,
                //OrderId = detaislMV.OrderID,
                //PackageId = detaislsMV.PackageID,
                CreatedTime = DateTime.Now,
            };
            await _unitOfWork.GetRepository<OrdersDetails>().InsertAsync(details);
            await _unitOfWork.SaveAsync();

            // Thêm mối quan hệ giữa OrderDetail và Packages
            foreach (var package in packages)
            {
                var orderDetailPackage = new OrderDetailPackage
                {
                    OrderDetailId = details.Id,
                    PackageId = package.Id,
                };
                await _unitOfWork.GetRepository<OrderDetailPackage>().InsertAsync(orderDetailPackage);
            }

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
            //var orDetails = await _unitOfWork.GetRepository<OrdersDetails>().GetAllAsync();
            var orDetails = await _unitOfWork.GetRepository<OrdersDetails>()
                                .Entities
                                .Include(od => od.Packages!)
                                .ToListAsync();
            if (orDetails == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Not found OrderDetail");
            }


            if (orDetails == null || !orDetails.Any())
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Not found OrderDetail");
            }

            var orDViewModels = orDetails.Select(orD => new GETOrderDetailModelView
            {
                Id = orD.Id,
                Quantity = orD.Quantity,
                Price = orD.Price,
                Status = orD.Status,
                //OrderID = orD.OrderId,
                CreatedTime = orD.CreatedTime
               
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
