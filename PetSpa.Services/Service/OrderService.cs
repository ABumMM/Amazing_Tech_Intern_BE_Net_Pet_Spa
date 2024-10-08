using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Repositories.IUOW;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.ModelViews.ModelViews;
using PetSpa.ModelViews.OrderModelViews;

namespace PetSpa.Services.Service
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //public async Task Add(PostOrderViewModel order)
        //{
        //    if (string.IsNullOrEmpty(order.PaymentMethod))
        //        throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.Validated, "PaymentMethod is required.");

        //    if (order.Total <= 0)
        //        throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.Validated, "Total must be greater than zero.");

        //    // Kiểm tra từng PackageID
        //    var orderDetails = await _unitOfWork.GetRepository<OrdersDetails>()
        //                                          .Entities
        //                                          .Where(p => order.OrderDetailId.Contains(p.Id))
        //                                          .ToListAsync();

        //    if (orderDetails.Count != order.OrderDetailId.Count)
        //    {
        //        throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "One or more PackageIDs not found.");
        //    }

        //    Orders newOrder = new Orders
        //    {
        //        PaymentMethod = order.PaymentMethod,
        //        Total = order.Total,
        //        CreatedTime = DateTime.Now,
        //    };

        //    var repository = _unitOfWork.GetRepository<Orders>();
        //    await repository.InsertAsync(newOrder);
        //    await _unitOfWork.SaveAsync();

        //    // Thêm mối quan hệ giữa Order và OrderDetail
        //    foreach (var orderDetail in orderDetails)
        //    {
        //        var orderDetailPackage = new OrderDetailPackage
        //        {
        //            Id = newOrder.Id, // Gán ID của Order mới
        //            OrderDetailId = orderDetail.Id // Gán ID của OrderDetail
        //        };
        //        await _unitOfWork.GetRepository<OrderDetailPackage>().InsertAsync(orderDetailPackage);
        //    }

        //    await _unitOfWork.SaveAsync();
        //}

        public async Task<BasePaginatedList<GetOrderViewModel>> GetAll(int pageNumber = 1, int pageSize = 3)
        {
            var orders = await _unitOfWork.GetRepository<Orders>().GetAllAsync();

            if (orders == null || !orders.Any())
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound,"Orders not found.");
            }

            var orderResponseList = orders.Select(order => new GetOrderViewModel
            {
                //UserId = order.UserId,
                Id = order.Id,
                PaymentMethod = order.PaymentMethod,
                Total = order.Total,
                CreatedTime = order.CreatedTime,
            }).ToList();

            var paginatedMemberShips = orderResponseList
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            // Truyền vào đầy đủ 4 tham số: items, totalItems, pageNumber, pageSize
            return new BasePaginatedList<GetOrderViewModel>(orderResponseList, orderResponseList.Count, pageNumber, pageSize);
        }

        public async Task<GetOrderViewModel?> GetById(string id)
        {
            var order = await _unitOfWork.GetRepository<Orders>().GetByIdAsync(id);
            if (order == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Orders not found.");
            }

            return new GetOrderViewModel
            {
                //UserId = order.UserId,
                Id = order.Id,
                PaymentMethod = order.PaymentMethod,
                Total = order.Total,
                CreatedTime = order.CreatedTime,
            };
        }

        public async Task Add(PostOrderViewModel order)
        {
            if (string.IsNullOrWhiteSpace(order.PaymentMethod))             
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.Validated, "PaymentMethod is required.");

            if (order.Total <= 0)
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.Validated, "Total must be greater than zero.");

            Orders newOrder = new Orders
            {
                PaymentMethod = order.PaymentMethod,
                Total = order.Total,
                CreatedTime = DateTime.Now,
            };
            await _unitOfWork.GetRepository<Orders>().InsertAsync(newOrder);
            await _unitOfWork.SaveAsync();
        }

        public async Task Update(PutOrderViewModel Order)
        {
            var order = await _unitOfWork.GetRepository<Orders>().GetByIdAsync(Order.Id);
            if (order == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Orders not found.");
            }

            order.PaymentMethod = Order.PaymentMethod ?? order.PaymentMethod;
            order.Total = (double)Order.Total;
            order.LastUpdatedTime = DateTime.Now;

            var repository = _unitOfWork.GetRepository<Orders>();
            await repository.UpdateAsync(order);
            await _unitOfWork.SaveAsync();
        }

        public async Task Delete(string id)
        {
            var repository = _unitOfWork.GetRepository<Orders>();
            var existingOrder = await repository.GetByIdAsync(id);

            if (existingOrder == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Orders not found.");
            }

            await repository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }

    }
}
