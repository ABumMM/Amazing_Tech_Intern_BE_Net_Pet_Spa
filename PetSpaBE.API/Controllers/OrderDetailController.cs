using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;

namespace PetSpaBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailService _orderDetailService;

        public OrderDetailController(IOrderDetailService orderDetailService) 
        {
            _orderDetailService = orderDetailService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrderDetail(int pageNumber = 1, int pageSize = 2)
        {
            IList<OrdersDetails> ordersDetails = await _orderDetailService.getAll();
            
            int totalOrderDetail = ordersDetails.Count;

            //Phân trang 
            var paginatedRoles = ordersDetails
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Tạo đối tượng BasePaginatedList để trả về
            var paginatedList = new BasePaginatedList<OrdersDetails>(paginatedRoles, totalOrderDetail, pageNumber, pageSize);

            return Ok(paginatedList);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrderDetail(OrdersDetails ordersDetails)
        {
            await _orderDetailService.Add(ordersDetails);
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> getOrderDetailByID(string id)
        {
            var orD = await _orderDetailService.getById(id) ?? null;
            if (orD is null)
                return NotFound("OrderDetail not found!");
            return Ok(orD);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOrderDetail(OrdersDetails orderDetails)
        {
            try
            {
                await _orderDetailService.Update(orderDetails);
                return Ok();
            }
            catch
            {
                return NotFound("OrderDetail not found!");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteOrderDetail(string id)
        {
            try
            {
                await _orderDetailService.Delete(id);
                return Ok();
            } 
            catch
            {
                return NotFound("OrderDetail not found!");
            }
        }

       
    }
}
