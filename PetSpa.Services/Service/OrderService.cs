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
            var orders = await _unitOfWork.GetRepository<Orders>()
                .GetAllAsync(); // Assuming GetAllAsync handles pagination internally

            if (orders == null || !orders.Any())
            {
                return new BasePaginatedList<GetOrderViewModel>(new List<GetOrderViewModel>(), 0);
            }

            var orderResponseList = orders.Select(order => new GetOrderViewModel
            {
                Id = order.Id,
                CustomerID = order.CustomerID,
                EmployeeID = order.EmployeeID,
                Date = order.Date,
                PaymentMethod = order.PaymentMethod,
                Total = order.Total,
            }).ToList();

            return new BasePaginatedList<GetOrderViewModel>(orderResponseList, orderResponseList.Count);
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
                Id = order.Id,
                CustomerID = order.CustomerID,
                EmployeeID = order.EmployeeID,
                Date = order.Date,
                PaymentMethod = order.PaymentMethod,
                Total = order.Total,
            };
        }

        public async Task Add(PostOrderViewModel order)
        {
            Orders newOrder = new Orders
            {
                Id = Guid.NewGuid().ToString("N"),
                CustomerID = order.CustomerID,
                EmployeeID = order.EmployeeID,
                Date = order.Date ?? DateTime.Now,
                PaymentMethod = string.IsNullOrEmpty(order.PaymentMethod) ? "Unknown" : order.PaymentMethod,
                Total = order.Total,
                CreatedTime = DateTime.Now,
            };

            var repository = _unitOfWork.GetRepository<Orders>();
            await repository.InsertAsync(newOrder);
            await _unitOfWork.SaveAsync();
        }

        public async Task Update(Orders order)
        {
            var existingOrder = await _unitOfWork.GetRepository<Orders>().GetByIdAsync(order.Id);
            if (existingOrder == null)
            {
                return; // No action needed if not found
            }

            existingOrder.CustomerID = order.CustomerID;
            existingOrder.EmployeeID = order.EmployeeID;
            existingOrder.Date = order.Date;
            existingOrder.PaymentMethod = order.PaymentMethod;
            existingOrder.Total = order.Total;

            var repository = _unitOfWork.GetRepository<Orders>();
            repository.Update(existingOrder);
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
