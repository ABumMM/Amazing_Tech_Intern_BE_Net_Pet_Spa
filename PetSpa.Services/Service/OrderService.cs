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

        public async Task<BasePaginatedList<GetOrderViewModel>> GetAll(int pageNumber = 1, int pageSize = 3)
        {
            var orders = await _unitOfWork.GetRepository<Orders>().GetAllAsync(); 
            if (orders == null || !orders.Any())
            {
                // Truyền vào đầy đủ 4 tham số: items, totalItems, pageNumber, pageSize
                return new BasePaginatedList<GetOrderViewModel>(new List<GetOrderViewModel>(), 0, pageNumber, pageSize);
            }

            var orderResponseList = orders.Select(order => new GetOrderViewModel
            {
                UserId = order.UserId,
                PaymentMethod = order.PaymentMethod,
                Total = order.Total,
                CreatedTime = order.CreatedTime,
            }).ToList();

            // Truyền vào đầy đủ 4 tham số: items, totalItems, pageNumber, pageSize
            return new BasePaginatedList<GetOrderViewModel>(orderResponseList, orderResponseList.Count, pageNumber, pageSize);
        }

        public async Task<GetOrderViewModel?> GetById(string id)
        {
            var order = await _unitOfWork.GetRepository<Orders>().GetByIdAsync(id);
            if (order == null)
            {
                return null; 
            }

            return new GetOrderViewModel
            {
                UserId = order.UserId,
                PaymentMethod = order.PaymentMethod,
                Total = order.Total,
                CreatedTime = order.CreatedTime,
            };
        }

        public async Task Add(PostOrderViewModel order)
        {
            Orders newOrder = new Orders
            {   
                PaymentMethod = string.IsNullOrEmpty(order.PaymentMethod) ? "Unknown" : order.PaymentMethod,
                Total = order.Total,
                CreatedTime = DateTime.Now,
            };

            var repository = _unitOfWork.GetRepository<Orders>();
            await repository.InsertAsync(newOrder);
            await _unitOfWork.SaveAsync();
        }

        public async Task Update(PutOrderViewModel Order)
        {
            var order = await _unitOfWork.GetRepository<Orders>().GetByIdAsync(Order.Id);
            if (order == null)
            {
                throw new Exception("Order not found");
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
                return; // No action needed if not found
            }

            await repository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }

    }
}
