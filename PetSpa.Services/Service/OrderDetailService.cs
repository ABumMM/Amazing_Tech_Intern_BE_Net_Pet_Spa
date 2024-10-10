using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Repositories.IUOW;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.Core.Infrastructure;
using PetSpa.Core.Utils;
using PetSpa.ModelViews.OrderDetailModelViews;
using PetSpa.ModelViews.PackageModelViews;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

namespace PetSpa.Services.Service
{
    public class OrderDetailService : IOrderDetailServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private string currentUserId => Authentication.GetUserIdFromHttpContextAccessor(_contextAccessor);
        public OrderDetailService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
        }

        public async Task Add(POSTOrderDetailModelView detailsMV)
        {
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

            if (detailsMV.PackageIDs == null || !detailsMV.PackageIDs.Any())
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "At least one PackageID is required.");
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
                Price = detailsMV.Price,
                Status = detailsMV.Status,
                OrderID = detailsMV.OrderID,
                CreatedTime = TimeHelper.ConvertToUtcPlus7(DateTime.Now),
                CreatedBy = currentUserId
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
            existedOrDetail.DeletedTime = TimeHelper.ConvertToUtcPlus7(DateTime.Now);
            await _unitOfWork.GetRepository<OrdersDetails>().DeleteAsync(OrDetailID);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeletePackageInOrDetail(string packageINOrDetailID)
        {
            OrderDetailPackage? exitstPackageInOrDetail = await _unitOfWork.GetRepository<OrderDetailPackage>().GetByIdAsync(packageINOrDetailID);
            if (exitstPackageInOrDetail == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Not found Package");
            }
            await _unitOfWork.GetRepository<OrderDetailPackage>().DeleteAsync(packageINOrDetailID);
            await _unitOfWork.SaveAsync();
        }

        public async Task<BasePaginatedList<GETOrderDetailModelView>> GetAll(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.InvalidInput, "Pagenumber and pagesize must greater than 0");
            }
            IQueryable<OrdersDetails> orDetails = _unitOfWork.GetRepository<OrdersDetails>()
                .Entities.Where(i => !i.DeletedTime.HasValue)
                .OrderByDescending(c => c.CreatedTime).AsQueryable();

            var paginatedOrDetail = await orDetails
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(orD => new GETOrderDetailModelView
                {
                    Id = orD.Id,
                    Quantity = orD.Quantity,
                    Price = (decimal)orD.Price,
                    Status = orD.Status,
                    OrderID = orD.OrderID,
                    CreatedTime = orD.CreatedTime,
                    CreatedBy = orD.CreatedBy,
                    ListPackage = orD.OrderDetailPackages.Select(odp => new GETPackageModelView_OrderDetails
                    {
                        Id = odp.Package.Id,
                        Name = odp.Package.Name,
                        Price = (decimal)odp.Package.Price,
                        Image = odp.Package.Image,
                        Information = odp.Package.Information,
                        Experiences = odp.Package.Experiences,
                    }).ToList()
                }).ToListAsync();
            return new BasePaginatedList<GETOrderDetailModelView>(paginatedOrDetail, await orDetails.CountAsync(), pageNumber, pageSize);
        }

        public async Task<GETOrderDetailModelView?> GetById(string? OrDetailID)
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
                Price = (decimal)existedOrDetail.Price,
                CreatedTime = existedOrDetail.CreatedTime
            };
        }

        public async Task<List<GETOrderDetailModelView>?> GETOrderDetailByConditions(DateTimeOffset? DateStart, DateTimeOffset? DateEnd)
        {
            // Khởi tạo truy vấn cho bảng Packages
            IQueryable<OrdersDetails> query = _unitOfWork.GetRepository<OrdersDetails>()
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
            var orDetails = await query.ToListAsync();

            // Kiểm tra nếu không tìm thấy gói nào
            if (!orDetails.Any())
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "No OrderDetail found.");
            }

            // Chuyển đổi dữ liệu sang GETPackageModelView
            var orDetailViewModels = orDetails.Select(orD => new GETOrderDetailModelView
            {
                Id = orD.Id,
                Quantity = orD.Quantity,
                Price = (decimal)orD.Price,
                Status = orD.Status,
                OrderID = orD.OrderID,
                CreatedTime = orD.CreatedTime,
                CreatedBy = orD.CreatedBy,
                ListPackage = orD.OrderDetailPackages.Select(odp => new GETPackageModelView_OrderDetails
                {
                    Id = odp.Package.Id,
                    Name = odp.Package.Name,
                    Price = (decimal)odp.Package.Price,
                    Image = odp.Package.Image,
                    Information = odp.Package.Information,
                    Experiences = odp.Package.Experiences,
                }).ToList()
            }).ToList();
            return orDetailViewModels;
        }

        public async Task Update(PUTOrderDetailModelView detailsMV)
        {
            if (detailsMV == null)
            {
                throw new BadRequestException(ErrorCode.BadRequest, "OrderDetail cannot null.");
            }
            if (detailsMV.Id == null)
            {
                throw new BadRequestException(ErrorCode.BadRequest, "OrderDetail Id is require null.");
            }

            OrdersDetails? existedOrDetail = await _unitOfWork.GetRepository<OrdersDetails>().GetByIdAsync(detailsMV.Id);

            if (existedOrDetail == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Not found OrderDetail");
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
            existedOrDetail.LastUpdatedBy = currentUserId;

            await _unitOfWork.GetRepository<OrdersDetails>().UpdateAsync(existedOrDetail);
            await _unitOfWork.SaveAsync();
        }
    }
}
