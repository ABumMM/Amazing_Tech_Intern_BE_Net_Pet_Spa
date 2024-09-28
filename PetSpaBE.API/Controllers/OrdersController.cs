using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.Core.Store;
using PetSpa.ModelViews.ModelViews;
using PetSpa.ModelViews.OrderModelViews;
using PetSpa.Services.Service;
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
            var orders = await _orderService.GetAll(pageNumber, pageSize);
            return Ok(new BaseResponseModel<BasePaginatedList<GetOrderViewModel>>(
                statusCode: (int)StatusCodeHelper.OK, // Sử dụng StatusCodeHelper cho mã trạng thái
                code: nameof(StatusCodeHelper.OK),   // Sử dụng tên của enum làm mã code
                data: orders));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(string id)
        {
            var order = await _orderService.GetById(id);
            if (order == null)
            {
                return NotFound(new BaseResponseModel<string>(
                    statusCode: (int)StatusCodeHelper.BadRequest,
                    code: nameof(StatusCodeHelper.BadRequest),
                    data: "Order not found"));
            }

            return Ok(new BaseResponseModel<GetOrderViewModel>(
                statusCode: (int)StatusCodeHelper.OK,
                code: nameof(StatusCodeHelper.OK),
                data: order));
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] PostOrderViewModel order)
        {
            await _orderService.Add(order);
            return Ok(new BaseResponseModel<string>(
                statusCode: (int)StatusCodeHelper.OK,
                code: nameof(StatusCodeHelper.OK),
                data: "Order created successfully"));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(string id, [FromBody] PutOrderViewModel Order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new BaseResponseModel<string>(
                    statusCode: (int)StatusCodeHelper.BadRequest,
                    code: nameof(StatusCodeHelper.BadRequest),
                    data: "Invalid order data"));
            }

            var existingOrder = await _orderService.GetById(id);
            if (existingOrder == null)
            {
                return NotFound(new BaseResponseModel<string>(
                    statusCode: (int)StatusCodeHelper.BadRequest,
                    code: nameof(StatusCodeHelper.BadRequest),
                    data: "Order not found"));
            }

            await _orderService.Update(Order);
            return Ok(new BaseResponseModel<string>(
                statusCode: (int)StatusCodeHelper.OK,
                code: nameof(StatusCodeHelper.OK),
                data: "Order updated successfully"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(string id)
        {
            await _orderService.Delete(id);   
            return Ok(new BaseResponseModel<string>(
                statusCode: (int)StatusCodeHelper.OK,
                code: nameof(StatusCodeHelper.OK),
                data: "Order deleted successfully"));
        }
    }
}
