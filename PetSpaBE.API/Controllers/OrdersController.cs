using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.ModelViews.ModelViews;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PetSpaBE.API.Controllers
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

        [HttpGet]
        public async Task<IActionResult> GetAllOrders(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var orders = await _orderService.GetAll();
                if (orders == null || !orders.Any())
                {
                    return NotFound("Không thể tìm thấy đơn hàng.");
                }

                var paginatedOrders = orders
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var totalOrdersCount = orders.Count;
                var paginatedList = new BasePaginatedList<OrderResponseModel>(paginatedOrders, totalOrdersCount, pageNumber, pageSize);

                return Ok(paginatedList);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(string id)
        {
            try
            {
                var order = await _orderService.GetById(id);
                if (order == null)
                {
                    return NotFound("Không thể tìm thấy đơn hàng.");
                }
                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] OrderResponseModel order)
        {
            try
            {
                await _orderService.Add(order);
                return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOrder([FromBody] OrderResponseModel order)
        {
            try
            {
                await _orderService.Update(order);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(string id)
        {
            try
            {
                await _orderService.Delete(id);
                return Ok("Đã xóa đơn hàng thành công");
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Không thể tìm thấy đơn hàng.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }
    }
}
