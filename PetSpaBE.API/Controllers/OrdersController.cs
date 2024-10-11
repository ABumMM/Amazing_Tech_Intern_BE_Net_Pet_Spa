﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.ModelViews.OrderModelViews;

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

        // Get All Orders
        [HttpGet]
        public async Task<IActionResult> GetAllOrders(int pageNumber , int pageSize )
        {
            var orders = await _orderService.GetAll(pageNumber, pageSize);
            return Ok(new BaseResponseModel<BasePaginatedList<GetOrderViewModel>>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: orders));
        }

        // Get Order by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(string id)
        {
            var order = await _orderService.GetById(id);
            return Ok(new BaseResponseModel<GetOrderViewModel>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: order));
        }

        // Add Order
        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] PostOrderViewModel order)
        {
            await _orderService.Add(order);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Order created successfully"));
        }

        // Update Order
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(string id, [FromBody] PutOrderViewModel order)
        {  
            var existingOrder = await _orderService.GetById(id);
            await _orderService.Update(order);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Order updated successfully"));
        }

        // Delete Order
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(string id)
        {
            var existingOrder = await _orderService.GetById(id);
            await _orderService.Delete(id);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Order deleted successfully"));
        }
    }
}
