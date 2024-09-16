using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;


namespace PetSpaBE.API.Controllers
{
    public class OrdersController : ControllerBase
    {
        [Route("api/[controller]")]
        [ApiController]
        public class OrdersController : ControllerBase
        {
            private readonly IOrderService _orderService;

            public OrdersController(IOrderService orderService)
            {
                _orderService = orderService;
            }
            // Hiển thị hết tất cả các đơn hàng
            [HttpGet]
            public async Task<IActionResult> GetAllOrders()
            {
                try
                {
                    IList<Orders> orders = await _orderService.GetAll();
                    return Ok(orders);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, $"Lỗi: {ex.Message}");
                }
            }

            // Tìm một đơn hàng theo ID
            [HttpGet("{id}")]
            public async Task<IActionResult> GetOrderById(string id)
            {
                try
                {
                    var order = await _orderService.GetById(id);
                    if (order == null)
                    {
                        return NotFound("Không thể tìm thấy đơn hàng");
                    }
                    return Ok(order);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, $"Lỗi: {ex.Message}");
                }
            }

            // Thêm mới một đơn hàng
            [HttpPost]
            public async Task<IActionResult> AddOrder(Orders order)
            {
                try
                {
                    await _orderService.Add(order);
                    return Ok("Đặt hàng thành công");
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, $"Lỗi: {ex.Message}");
                }
            }

            // Cập nhật một đơn hàng
            [HttpPut]
            public async Task<IActionResult> UpdateOrder(Orders order)
            {
                try
                {
                    await _orderService.Update(order);
                    return Ok("Cập nhật đơn hàng thành công");
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, $"Lỗi: {ex.Message}");
                }
            }

            // Xóa một đơn hàng theo ID
            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteOrder(string id)
            {
                try
                {
                    await _orderService.Delete(id);
                    return Ok("Xóa đơn hàng thành công");
                }
                catch (KeyNotFoundException)
                {
                    return NotFound("Không tìm thấy đơn hàng này");
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, $"Lỗi: {ex.Message}");
                }
            }
        }
    }
}
