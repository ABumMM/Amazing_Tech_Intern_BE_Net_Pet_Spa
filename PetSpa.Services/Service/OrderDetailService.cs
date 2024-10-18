using AutoMapper;
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
        private readonly IMapper _mapper;

        private string currentUserId => Authentication.GetUserIdFromHttpContextAccessor(_contextAccessor);
        public OrderDetailService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _mapper = mapper;
        }

        public async Task Add(POSTOrderDetailModelView detailsMV)
        {
            if (detailsMV.Status == null)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "OrderDetail Status is required.");
            }
            if (detailsMV.Quantity < 0)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Quantity must be greater than or equal to 0.");
            }

            // Kiểm tra từng PackageID
            var package = _unitOfWork.GetRepository<Packages>()
                         .Entities.FirstOrDefault(p => detailsMV.PackageID.Contains(p.Id));

            decimal totalPrice = 0;
            if (package != null)
            {
                totalPrice += package.Price * detailsMV.Quantity;
            }
            var user = await _unitOfWork.GetRepository<ApplicationUser>().GetByIdAsync(Guid.Parse(currentUserId));


            OrdersDetails details = new OrdersDetails()
            {
                Quantity = (int)detailsMV.Quantity,
                Price = totalPrice,
                Status = detailsMV.Status,
                PackageID=detailsMV.PackageID,
                CreatedTime = TimeHelper.ConvertToUtcPlus7(DateTime.Now),
                CreatedBy = user?.UserName
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
            existedOrDetail.DeletedTime = TimeHelper.ConvertToUtcPlus7(DateTime.Now);
            await _unitOfWork.GetRepository<OrdersDetails>().DeleteAsync(OrDetailID);
            await _unitOfWork.SaveAsync();
        } 

        public async Task<BasePaginatedList<GETOrderDetailModelView>> GetAll(string orderID,int pageNumber, int pageSize)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.InvalidInput, "Pagenumber and pagesize must greater than 0");
            }
            IQueryable<OrdersDetails> orDetails = _unitOfWork.GetRepository<OrdersDetails>()
                .Entities.Where(i => !i.DeletedTime.HasValue 
                    && i.OrderID== orderID)
                .OrderByDescending(c => c.CreatedTime).AsQueryable();

            var paginatedOrDetail = await orDetails
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                //.Select(orD => new GETOrderDetailModelView
                //{
                //    Id = orD.Id,
                //    Quantity = orD.Quantity,
                //    Price = (decimal)orD.Price,
                //    Status = orD.Status,
                //    //OrderID = orD.Orders.Id,
                //    CreatedTime = orD.CreatedTime,
                //    CreatedBy = orD.CreatedBy,
                //})
                .ToListAsync();
            //return new BasePaginatedList<GETOrderDetailModelView>(paginatedOrDetail, await orDetails.CountAsync(), pageNumber, pageSize);
            return new BasePaginatedList<GETOrderDetailModelView>(_mapper.Map<List<GETOrderDetailModelView>>(paginatedOrDetail), await orDetails.CountAsync(), pageNumber, pageSize);
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

            //return new GETOrderDetailModelView
            //{
            //    Id = existedOrDetail.Id,
            //    Quantity = existedOrDetail.Quantity,
            //    Status = existedOrDetail.Status,
            //    Price = (decimal)existedOrDetail.Price,
            //    CreatedTime = existedOrDetail.CreatedTime
            //};
            return _mapper.Map<GETOrderDetailModelView>(existedOrDetail);
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
            //var orDetailViewModels = orDetails.Select(orD => new GETOrderDetailModelView
            //{
            //    Id = orD.Id,
            //    Quantity = orD.Quantity,
            //    Price = (decimal)orD.Price,
            //    Status = orD.Status,
            //    OrderID = orD.Orders.Id,
            //    CreatedTime = orD.CreatedTime,
            //    CreatedBy = orD.CreatedBy,
                
            //}).ToList();
            //return orDetailViewModels;
            return _mapper.Map<List<GETOrderDetailModelView>>(orDetails);
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
            if (detailsMV.Quantity < 0)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Quantity must be greater than or equal to 0.");
            }

            //existedOrDetail.Quantity = (int)detailsMV.Quantity;
            //existedOrDetail.Status = detailsMV.Status;
            //existedOrDetail.LastUpdatedTime = DateTime.Now;
            //existedOrDetail.LastUpdatedBy = currentUserId;
            _mapper.Map(detailsMV, existedOrDetail);
            await _unitOfWork.GetRepository<OrdersDetails>().UpdateAsync(existedOrDetail);
            await _unitOfWork.SaveAsync();
        }
    }
}
