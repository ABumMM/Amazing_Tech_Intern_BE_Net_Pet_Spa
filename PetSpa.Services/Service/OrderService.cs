﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Repositories.IUOW;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.Core.Infrastructure;
using PetSpa.ModelViews.MemberShipModelView;
using PetSpa.ModelViews.ModelViews;
using PetSpa.ModelViews.OrderDetailModelViews;
using PetSpa.ModelViews.OrderModelViews;
using PetSpa.ModelViews.PackageModelViews;
using System.Security.Cryptography;

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
                .Select(o => new GetOrderViewModel
                {
                    Id = o.Id,
                    PaymentMethod = o.PaymentMethod,
                    Total = o.Total,
                    CreatedBy = o.CreatedBy,
                    CreatedTime = o.CreatedTime,     
                //    OrderDetailId = o.OrderDetails.Any()
                //? o.OrderDetails.Select(od => new GETOrderDetailModelView
                //{
                //    Id = od.Id,
                //    Quantity = od.Quantity,
                //    Status = od.Status,
                //    Price = od.Price ?? 0, // Xử lý null cho Price
                //    CreatedBy = od.CreatedBy,
                //    CreatedTime = od.CreatedTime
                //}).ToList()
                //: new List<GETOrderDetailModelView>() // Trả về danh sách trống nếu không có chi tiết đơn hàng
                }).ToListAsync();
            return new BasePaginatedList<GetOrderViewModel>(paginatedOrders, await orders.CountAsync(), pageNumber, pageSize);
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

            return new GetOrderViewModel
            {
                Id = order.Id,
                PaymentMethod = order.PaymentMethod,
                Total = order.Total,
                IsPaid = order.IsPaid,
                CreatedTime = order.CreatedTime
            };
        }

        public async Task Add(PostOrderViewModel order)
        {
            if (string.IsNullOrWhiteSpace(order.PaymentMethod))
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.Validated, "PaymentMethod is required.");

            // Kiểm tra xem OrderDetailId có giá trị không
            if (order.OrderDetailId == null || !order.OrderDetailId.Any())
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.Validated, "OrderDetailId is required.");

            // Lấy danh sách chi tiết đơn hàng dựa trên OrderDetailId
            var orderDetails = await _unitOfWork.GetRepository<OrdersDetails>()
                .Entities
                .Where(od => order.OrderDetailId.Contains(od.Id)) 
                .ToListAsync();

            // Tính tổng số tiền cho các chi tiết đơn hàng
            decimal totalAmount = 0;
            foreach (var detail in orderDetails)
            {
                    totalAmount += detail.Price; // Tính tổng
            }
            // Lấy thông tin khách hàng (membership)
            var membership = await _unitOfWork.GetRepository<MemberShips>()
                .Entities.FirstOrDefaultAsync(m => m.UserId == Guid.Parse(currentUserId) 
                && !m.DeletedTime.HasValue);

            double discountedTotal = 0;
            if (membership != null)
            {
                // Tính tổng tiền sau khi áp dụng giảm giá
                 discountedTotal =(double)totalAmount * (1 - membership.DiscountRate);
            }
            else
            {
                discountedTotal = (double)totalAmount; // Nếu không có thành viên, sử dụng tổng gốc
            }

            // Tạo đơn hàng mới với tổng đã tính
            Orders newOrder = new Orders
            {
                PaymentMethod = order.PaymentMethod,
                Total =discountedTotal, // Sử dụng tổng đã tính
                IsPaid = false, // Đơn hàng mới tạo mặc định là chưa thanh toán
                CreatedBy = currentUserId,
                CreatedTime = DateTime.Now,
            };
            await _unitOfWork.GetRepository<Orders>().InsertAsync(newOrder);
            await _unitOfWork.SaveAsync();

            // Cập nhật số tiền đã sử dụng của khách hàng nếu là thành viên
            if (membership != null)
            {
                // Cộng tổng số tiền trước khi giảm giá vào TotalSpent
                membership.TotalSpent +=(double) totalAmount; 
                await _unitOfWork.GetRepository<MemberShips>().UpdateAsync(membership);
                await _unitOfWork.SaveAsync();
                await CheckMembershipUpgrade(Guid.Parse(currentUserId));
            }
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
            existingOrder.PaymentMethod = order.PaymentMethod ?? existingOrder.PaymentMethod;
            existingOrder.Total = order.Total; // Có thể cần xử lý kiểm tra giá trị
            existingOrder.LastUpdatedTime = DateTime.UtcNow; // Sử dụng UTC
            existingOrder.LastUpdatedBy = currentUserId; // Cập nhật thông tin người dùng hiện tại
            await _unitOfWork.GetRepository<Orders>().UpdateAsync(existingOrder);
            await _unitOfWork.SaveAsync();
        }

        public async Task Delete(string id)
        {
            // Kiểm tra xem đơn hàng có tồn tại không
            var existingOrder = await _unitOfWork.GetRepository<Orders>().GetByIdAsync(id);
            if (existingOrder == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Orders not found.");
            }
            // Đánh dấu thời gian và người thực hiện xóa (xóa mềm)
            existingOrder.DeletedTime = DateTime.Now;
            existingOrder.DeletedBy = "Anh Nguyen"; // Cập nhật với thông tin người dùng hiện tại

            // Cập nhật thông tin vào cơ sở dữ liệu
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
                .Select(o => new GetOrderViewModel
                {
                    Id = o.Id,
                    PaymentMethod = o.PaymentMethod,
                    Total = o.Total,
                    CreatedBy = o.CreatedBy,
                    CreatedTime = o.CreatedTime,
                    IsPaid = o.IsPaid
                }).ToListAsync();

            return new BasePaginatedList<GetOrderViewModel>(paginatedOrders, await orders.CountAsync(), pageNumber, pageSize);
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
                    .FirstOrDefaultAsync(m => m.UserId == customerId && !m.DeletedTime.HasValue);

            if (membership == null)
            {
                throw new Exception("No active membership found for the customer.");
            }

            if (membership.TotalSpent >= 20000000) // Platinum
            {
                if (membership.Name != "Platinum")
                {
                    membership.Name = "Platinum";
                    membership.DiscountRate = 0.20; // Giảm giá 20% cho hạng Platinum
                }
            }
            else if (membership.TotalSpent >= 10000000) // Gold
            {
                if (membership.Name != "Gold")
                {
                    membership.Name = "Gold";
                    membership.DiscountRate = 0.15; // Giảm giá 15% cho hạng Gold
                }
            }
            else if (membership.TotalSpent >= 5000000) // Silver
            {
                if (membership.Name != "Silver")
                {
                    membership.Name = "Silver";
                    membership.DiscountRate = 0.10; // Giảm giá 10% cho hạng Silver
                }
            }
            else // Standard
            {
                if (membership.Name != "Standard")
                {
                    membership.Name = "Standard";
                    membership.DiscountRate = 0; // Không giảm giá cho hạng Standard
                }
            }

            await _unitOfWork.GetRepository<MemberShips>().UpdateAsync(membership);
            await _unitOfWork.SaveAsync();
        }
    }
}
