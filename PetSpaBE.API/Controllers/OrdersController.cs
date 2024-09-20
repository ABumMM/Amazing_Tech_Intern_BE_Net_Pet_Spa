using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.ModelViews.ModelViews;
using PetSpa.ModelViews.OrderModelViews;
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
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: orders));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(string id)
        {
            var order = await _orderService.GetById(id);
            if (order == null)
            {
                return NotFound(new BaseResponseModel<string>(
                    statusCode: StatusCodes.Status404NotFound,
                    code: ResponseCodeConstants.NOT_FOUND,
                    data: "Order not found"));
            }

            return Ok(new BaseResponseModel<GetOrderViewModel>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: order));
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] PostOrderViewModel order)
        {
            await _orderService.Add(order);
            return CreatedAtAction(nameof(GetOrderById), new { id = order.CustomerID }, new BaseResponseModel<string>(
                statusCode: StatusCodes.Status201Created,
                code: ResponseCodeConstants.SUCCESS,
                data: "Order created successfully"));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOrder(Orders order)
        {
            await _orderService.Update(order);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(string id)
        {
            await _orderService.Delete(id);   
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Order deleted successfully"));
        }
    }
}
