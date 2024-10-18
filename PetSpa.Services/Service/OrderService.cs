using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Repositories.IUOW;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.Core.Infrastructure;
using PetSpa.ModelViews.OrderModelViews;
namespace PetSpa.Services.Service
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMapper _mapper;

        private string currentUserId => Authentication.GetUserIdFromHttpContextAccessor(_contextAccessor);

        public OrderService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _mapper = mapper;
        }

        public async Task<BasePaginatedList<GetOrderViewModel>> GetAll(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Page number and page size must be greater than 0.");
            }

            IQueryable<Orders> orders = _unitOfWork.GetRepository<Orders>()
                .Entities.Where(o => !o.DeletedTime.HasValue)
                .OrderByDescending(o => o.CreatedTime)
                .AsQueryable();

            var paginatedOrders = await orders
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            // Sử dụng AutoMapper để map từ Orders sang GetOrderViewModel
            return new BasePaginatedList<GetOrderViewModel>(_mapper.Map<List<GetOrderViewModel>>(paginatedOrders), await orders.CountAsync(), pageNumber, pageSize);
        }


        public async Task<GetOrderViewModel?> GetById(string id)
        {
            // Kiểm tra tính hợp lệ của ID đơn hàng
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Invalid order ID.");
            }
            // Lấy đơn hàng theo ID
            var order = await _unitOfWork.GetRepository<Orders>()
                .Entities.FirstOrDefaultAsync(o => o.Id == id)
                ?? throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Not found Orders");
            return _mapper.Map<GetOrderViewModel>(order);
        }

        public async Task Add(PostOrderViewModel order)
        {

            if (string.IsNullOrWhiteSpace(order.PaymentMethod))
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.Validated, "PaymentMethod is required.");

            //if (order.OrderDetailId == null || !order.OrderDetailId.Any())
            //throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.Validated, "OrderDetailId is required.");

            //var orderDetails = await _unitOfWork.GetRepository<OrdersDetails>()
            //.Entities
            //.Where(od => order.OrderDetailId.Contains(od.Id))
            //.ToListAsync();

            //decimal totalAmount = orderDetails.Sum(detail => detail.Price);
            decimal totalAmount = 0;
            //var membership = await _unitOfWork.GetRepository<MemberShips>()
            //    .Entities.FirstOrDefaultAsync(m => m.UserId == Guid.Parse(order.CustomerID) && !m.DeletedTime.HasValue);

            //decimal discountedTotal = membership != null
            //? totalAmount * (decimal)(1 - membership.DiscountRate)
            //: totalAmount;

            var newOrder = _mapper.Map<Orders>(order);
            newOrder.Name = order.Name;
            newOrder.Total = totalAmount;
            newOrder.IsPaid = false;
            newOrder.CustomerID = Guid.Parse(order.CustomerID);
            newOrder.CreatedBy = currentUserId;
            newOrder.CreatedTime = DateTime.Now;

            await _unitOfWork.GetRepository<Orders>().InsertAsync(newOrder);
            await _unitOfWork.SaveAsync();

            //if (membership != null)
            //{
            //    //membership.TotalSpent += (double)totalAmount;
            //    await _unitOfWork.GetRepository<MemberShips>().UpdateAsync(membership);
            //    await _unitOfWork.SaveAsync();
            //    await CheckMembershipUpgrade(Guid.Parse(currentUserId));

            //    // Cập nhật OrderDetailID
            //    //var existedOrderDetails = await _unitOfWork.GetRepository<OrdersDetails>()
            //        //.Entities
            //        //.Where(od => order.OrderDetailId.Contains(od.Id))
            //        //.ToListAsync();

            //    //foreach (var detail in existedOrderDetails)
            //    //{
            //    //    detail.OrderID = newOrder.Id;
            //    //    await _unitOfWork.GetRepository<OrdersDetails>().UpdateAsync(detail);
            //    //}

            //    //await _unitOfWork.SaveAsync();
            //}
        }

        public async Task Update(PutOrderViewModel order)
        {
            if (order == null)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Order cannot be null.");
            }

            var existingOrder = await _unitOfWork.GetRepository<Orders>().GetByIdAsync(order.Id);
            if (existingOrder == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Order not found.");
            }
            // Lấy danh sách chi tiết đơn hàng để tính toán tổng
            var orderDetails = await _unitOfWork.GetRepository<OrdersDetails>()
                .Entities
                .Where(od => od.OrderID == existingOrder.Id)
                .ToListAsync();

            // Tính toán tổng từ các chi tiết đơn hàng
            //double totalAmount = (double)orderDetails.Sum(detail => detail.Price);
            // Cập nhật tổng cho đơn hàng
            //existingOrder.Total = totalAmount;
            _mapper.Map(order, existingOrder);
            existingOrder.LastUpdatedTime = DateTime.UtcNow;
            existingOrder.LastUpdatedBy = currentUserId;

            await _unitOfWork.GetRepository<Orders>().UpdateAsync(existingOrder);
            await _unitOfWork.SaveAsync();
        }

        public async Task Delete(string id)
        {
            var existingOrder = await _unitOfWork.GetRepository<Orders>().GetByIdAsync(id);
            if (existingOrder == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Orders not found.");
            }

            // Lấy danh sách các OrderDetails
            var orderDetails = await _unitOfWork.GetRepository<OrdersDetails>()
                .Entities
                .Where(od => od.OrderID == existingOrder.Id)
                .ToListAsync();

            // Xóa các OrderDetails bằng cách sử dụng ID
            foreach (var detail in orderDetails)
            {
                await _unitOfWork.GetRepository<OrdersDetails>().DeleteAsync(detail.Id);
            }

            // xóa mềm
            existingOrder.DeletedTime = DateTime.Now;
            existingOrder.DeletedBy = currentUserId.ToString();

            await _unitOfWork.GetRepository<Orders>().UpdateAsync(existingOrder);
            await _unitOfWork.SaveAsync();
        }


        public async Task<BasePaginatedList<GetOrderViewModel>> GetOrdersByPaymentStatus(bool isPaid, int pageNumber, int pageSize)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Page number and page size must be greater than 0.");
            }

            IQueryable<Orders> orders = _unitOfWork.GetRepository<Orders>()
                .Entities
                .Where(o => !o.DeletedTime.HasValue && o.IsPaid == isPaid)
                .OrderByDescending(o => o.CreatedTime)
                .AsQueryable();

            var paginatedOrders = await orders
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Sử dụng AutoMapper để ánh xạ
            return new BasePaginatedList<GetOrderViewModel>(_mapper.Map<List<GetOrderViewModel>>(paginatedOrders), await orders.CountAsync(), pageNumber, pageSize);
        }

        public async Task ConfirmOrder(string id)
        {
            var existingOrder = await _unitOfWork.GetRepository<Orders>().GetByIdAsync(id);
            if (existingOrder == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Order not found.");
            }

            existingOrder.IsPaid = true; // Xác nhận đơn hàng đã thanh toán
            existingOrder.LastUpdatedTime = DateTime.UtcNow;
            existingOrder.LastUpdatedBy = currentUserId;

            await _unitOfWork.GetRepository<Orders>().UpdateAsync(existingOrder);
            await _unitOfWork.SaveAsync();
        }

        // Phương thức kiểm tra xem thành viên có đủ điều kiện để nâng hạng không
        public async Task CheckMembershipUpgrade(Guid customerId)
        {
            var membership = await _unitOfWork.GetRepository<MemberShips>().Entities
                    .FirstOrDefaultAsync(m => m.UserId == customerId
                    && !m.DeletedTime.HasValue);

            if (membership == null)
            {
                throw new Exception("No active membership found for the customer.");
            }

            //if (membership.TotalSpent >= 20000000) // Platinum
            //{
            //    if (membership.Name != "Platinum")
            //    {
            //        membership.Name = "Platinum";
            //        membership.DiscountRate = 0.20; // Giảm giá 20% cho hạng Platinum
            //    }
            //}
            //else if (membership.TotalSpent >= 10000000) // Gold
            //{
            //    if (membership.Name != "Gold")
            //    {
            //        membership.Name = "Gold";
            //        membership.DiscountRate = 0.15; // Giảm giá 15% cho hạng Gold
            //    }
            //}
            //else if (membership.TotalSpent >= 5000000) // Silver
            //{
            //    if (membership.Name != "Silver")
            //    {
            //        membership.Name = "Silver";
            //        membership.DiscountRate = 0.10; // Giảm giá 10% cho hạng Silver
            //    }
            //}
            //else // Standard
            //{
            //    if (membership.Name != "Standard")
            //    {
            //        membership.Name = "Standard";
            //        membership.DiscountRate = 0; // Không giảm giá cho hạng Standard
            //    }
            //}
            await _unitOfWork.GetRepository<MemberShips>().UpdateAsync(membership);
            await _unitOfWork.SaveAsync();
        }
    }
}
