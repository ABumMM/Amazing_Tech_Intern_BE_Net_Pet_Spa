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

            if (orders == null || !orders.Any())
            {
                throw new KeyNotFoundException("Không có đơn hàng nào.");
            }

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
            if (id == null || string.IsNullOrEmpty(id.ToString()))
            {
                throw new ArgumentException("ID không hợp lệ.");
            }

            var order = await _unitOfWork.GetRepository<Orders>().GetByIdAsync(id);

            if (order == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy đơn hàng với ID: {id}");
            }

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
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order), "Thông tin đơn hàng không được để trống.");
            }

            if (order.CustomerID == null || order.EmployeeID == null)
            {
                throw new ArgumentException("CustomerID và EmployeeID là bắt buộc.");
            }

            if (order.Total <= 0)
            {
                throw new ArgumentException("Tổng tiền phải lớn hơn 0.");
            }

            var newOrder = new Orders
            {
                Id = Guid.NewGuid().ToString("N"),
                CustomerID = order.CustomerID,
                EmployeeID = order.EmployeeID,
                Date = order.Date,
                PaymentMethod = string.IsNullOrEmpty(order.PaymentMethod) ? "Unknown" : order.PaymentMethod,
                Total = order.Total,
            };

            var repository = _unitOfWork.GetRepository<Orders>();
            await repository.InsertAsync(newOrder);
            await _unitOfWork.SaveAsync();
        }

        public async Task Update(OrderResponseModel order)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order), "Thông tin đơn hàng không được để trống.");
            }

            var existingOrder = await _unitOfWork.GetRepository<Orders>().GetByIdAsync(order.Id);
            if (existingOrder == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy đơn hàng với ID: {order.Id}");
            }

            if (order.Total <= 0)
            {
                throw new ArgumentException("Tổng tiền phải lớn hơn 0.");
            }

            existingOrder.CustomerID = order.CustomerID;
            existingOrder.EmployeeID = order.EmployeeID;
            existingOrder.Date = order.Date;
            existingOrder.PaymentMethod = string.IsNullOrEmpty(order.PaymentMethod) ? existingOrder.PaymentMethod : order.PaymentMethod;
            existingOrder.Total = order.Total;

            var repository = _unitOfWork.GetRepository<Orders>();
            repository.Update(existingOrder);
            await _unitOfWork.SaveAsync();
        }

        public async Task Delete(object id)
        {
            if (id == null || string.IsNullOrEmpty(id.ToString()))
            {
                throw new ArgumentException("ID không hợp lệ.");
            }

            var repository = _unitOfWork.GetRepository<Orders>();
            var existingOrder = await repository.GetByIdAsync(id);

            if (existingOrder == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy đơn hàng với ID: {id}");
            }

            await repository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }
    }
}
