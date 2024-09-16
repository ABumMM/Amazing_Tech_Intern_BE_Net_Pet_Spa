using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Repositories.IUOW;
using PetSpa.Contract.Services.Interface;
using PetSpa.ModelViews.ModelViews;

namespace PetSpa.Services.Service
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IList<OrderResponseModel>> GetAll()
        {
            var orders = await _unitOfWork.GetRepository<Orders>().GetAllAsync();

            return orders.Select(order => new OrderResponseModel
            {
                Id = order.Id,
                CustomerID = order.CustomerID,
                EmployeeID = order.EmployeeID,
                Date = order.Date,
                PaymentMethod = order.PaymentMethod,
                Total = order.Total
            }).ToList();
        }

        public async Task<OrderResponseModel?> GetById(object id)
        {
            var order = await _unitOfWork.GetRepository<Orders>().GetByIdAsync(id);

            if (order == null)
                return null;

            return new OrderResponseModel
            {
                Id = order.Id,
                CustomerID = order.CustomerID,
                EmployeeID = order.EmployeeID,
                Date = order.Date,
                PaymentMethod = order.PaymentMethod,
                Total = order.Total,
            };
        }

        public async Task Add(OrderResponseModel order)
        {
            var newOrder = new Orders
            {
                Id = Guid.NewGuid().ToString("N"),
                CustomerID = order.CustomerID,
                EmployeeID = order.EmployeeID,
                Date = order.Date,
                PaymentMethod = order.PaymentMethod,
                Total = order.Total,
                // Chuyển đổi Bookings nếu cần
            };

            var repository = _unitOfWork.GetRepository<Orders>();
            await repository.InsertAsync(newOrder);
            await _unitOfWork.SaveAsync();
        }

        public async Task Update(OrderResponseModel order)
        {
            var existingOrder = new Orders
            {
                Id = order.Id,
                CustomerID = order.CustomerID,
                EmployeeID = order.EmployeeID,
                Date = order.Date,
                PaymentMethod = order.PaymentMethod,
                Total = order.Total,
                // Cập nhật Bookings nếu cần
            };

            var repository = _unitOfWork.GetRepository<Orders>();
            repository.Update(existingOrder);
            await _unitOfWork.SaveAsync();
        }

        public async Task Delete(object id)
        {
            var repository = _unitOfWork.GetRepository<Orders>();
            await repository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }
    }
}
