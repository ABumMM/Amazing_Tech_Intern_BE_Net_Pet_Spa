using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.ModelViews.OrderDetailModelViews;
using PetSpa.ModelViews.PackageModelViews;

namespace PetSpaBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailServices _orderDetailService;

        public OrderDetailController(IOrderDetailServices orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrderDetail(int pageNumber = 1, int pageSize = 2)
        {
            var orDetails = await _orderDetailService.getAll(pageNumber, pageSize);
            return Ok(new BaseResponseModel<BasePaginatedList<GETOrderDetailModelView>>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: orDetails));
        }

        [HttpPost]
        public async Task<IActionResult> AddOrderDetail([FromBody] POSTOrderDetailModelView detailsMV)
        {
            await _orderDetailService.Add(detailsMV);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Add OrderDetail successful"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderDetail(string id)
        {
            await _orderDetailService.Delete(id);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Delete OrderDetail successful"));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> getOrderDetailByID(string id)
        {
            var orDetails = await _orderDetailService.getById(id);
            return Ok(new BaseResponseModel<GETOrderDetailModelView>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: orDetails));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderDetail([FromBody] PUTOrderDetailModelView detailsMV)
        {
            await _orderDetailService.Update(detailsMV);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Update OrderDetail successful"));
        }




    }
}
