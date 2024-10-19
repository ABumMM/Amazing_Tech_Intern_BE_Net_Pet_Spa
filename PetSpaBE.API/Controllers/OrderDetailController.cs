using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.ModelViews.OrderDetailModelViews;
using PetSpa.ModelViews.PackageModelViews;
using PetSpa.Services.Service;

namespace PetSpaBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailServices _orderDetailService;
        private readonly IMembershipsService _membershipsService;
        public OrderDetailController(IOrderDetailServices orderDetailService , IMembershipsService membershipsService)
        {
            _orderDetailService = orderDetailService;
            _membershipsService = membershipsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrderDetail(int pageNumber, int pageSize)
        {
            var orDetails = await _orderDetailService.GetAll( pageNumber, pageSize);
           
            return Ok(new BaseResponseModel<BasePaginatedList<GETOrderDetailModelView>>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: orDetails));
        }

        [HttpPost]
        public async Task<IActionResult> AddOrderDetail([FromBody] POSTOrderDetailModelView detailsMV)
        {
            await _orderDetailService.Add(detailsMV);
            await _membershipsService.UpdateMemberShip(detailsMV.OrderID);
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
        [HttpGet("conditions")]
        public async Task<IActionResult> GetOrderDetailsByConditions(DateTimeOffset? DateStart, DateTimeOffset? DateEnd)
        {
            var orDetails = await _orderDetailService.GETOrderDetailByConditions(DateStart, DateEnd);
            return Ok(new BaseResponseModel<List<GETOrderDetailModelView>>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: orDetails));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderDetailByID(string id)
        {
            var orDetails = await _orderDetailService.GetById(id);
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
