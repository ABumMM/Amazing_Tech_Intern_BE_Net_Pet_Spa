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
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "OrderDetail Quantity must be greater than or equal to 0.");
            }

            if (detailsMV.OrderID == null)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "OrderDetail OrderID is required.");
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
                OrderID = detailsMV.OrderID,
                Quantity = (int)detailsMV.Quantity,
                Price = totalPrice,
                Status = detailsMV.Status,
                PackageID = detailsMV.PackageID,
                CreatedTime = TimeHelper.ConvertToUtcPlus7(DateTime.Now),
                CreatedBy = user?.UserName,
            };
            await _unitOfWork.GetRepository<OrdersDetails>().InsertAsync(details);

            await _unitOfWork.SaveAsync();
            //Update orderID trong orderDetailID
            var existedOrder = await _unitOfWork.GetRepository<Orders>()
                            .Entities
                            .Where(od => detailsMV.OrderID.Contains(od.Id))
                            .SingleOrDefaultAsync();
            if (existedOrder != null)
            {

                existedOrder.Total += details.Price;
                await _unitOfWork.GetRepository<Orders>().UpdateAsync(existedOrder);
                await _unitOfWork.SaveAsync();
                await UpdateTotalPriceOfUser(existedOrder.CustomerID.ToString(), totalPrice);
            }

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

            //Update orderID trong orderDetailID
            var existedOrder = await _unitOfWork.GetRepository<Orders>()
                            .Entities
                            .Where(od => existedOrDetail.OrderID.Contains(od.Id))
                            .SingleOrDefaultAsync();
            if (existedOrder != null)
            {

                existedOrder.Total -= existedOrDetail.Price;
                await _unitOfWork.GetRepository<Orders>().UpdateAsync(existedOrder);
                await _unitOfWork.SaveAsync();

            }
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
                .ToListAsync();
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
            _mapper.Map(detailsMV, existedOrDetail);
            await _unitOfWork.GetRepository<OrdersDetails>().UpdateAsync(existedOrDetail);
            await _unitOfWork.SaveAsync();
        }

        public async Task<decimal> CalculateTotalPrice(string orderId)
        {
            // Lấy tất cả OrderDetails hiện có cho OrderID
            var existingOrderDetails = await _unitOfWork.GetRepository<OrdersDetails>()
                                                .Entities.Where(od => od.OrderID == orderId && !od.DeletedTime.HasValue)
                                                .ToListAsync();

            // Tính tổng giá trị Price
            decimal totalPrice = existingOrderDetails.Sum(od => od.Price);

            return totalPrice;
        }

        public async Task UpdateOrderTotal(string orderId)
        {
            // Tính tổng giá trị Price cho tất cả OrderDetails liên quan đến OrderID
            decimal totalPrice = await CalculateTotalPrice(orderId);

            // Lấy Order hiện tại dựa trên OrderID
            var order = await _unitOfWork.GetRepository<Orders>().GetByIdAsync(orderId);
            if (order == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Order not found.");
            }

            // Gán giá trị tổng cho thuộc tính Total
            order.Total = totalPrice;

            // Cập nhật Order trong cơ sở dữ liệu
            await _unitOfWork.GetRepository<Orders>().UpdateAsync(order);
            await _unitOfWork.SaveAsync();

        }

        public async Task UpdateTotalPriceOfUser(string userId, decimal totalPriceOfOrderDetal)
        {
            var User = await _unitOfWork.GetRepository<MemberShips>().Entities.FirstOrDefaultAsync(m => m.UserId.ToString() == userId);
            if (User != null)
            {
                User.TotalSpent += totalPriceOfOrderDetal;
                await _unitOfWork.SaveAsync();
            }

        }
    }
}