using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.ModelViews.PackageModelViews;
using PetSpa.ModelViews.PaymentModelView;
using PetSpa.Services.Service;
using Swashbuckle.AspNetCore.Annotations;

namespace PetSpaBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IOrderService _orderService;

        public PaymentController(IPaymentService paymentService, IOrderService orderService)
        {
            _paymentService = paymentService;
            _orderService = orderService;
        }
        [HttpPost("PaymentCheckout")]
        [SwaggerOperation(Summary = "Create payment and return payment URL")]
        public async Task<IActionResult> PaymentCheckout([FromBody] PostPaymentModelView payment)
        {
            var paymentUrl = await _paymentService.CreatePaymentUrl(HttpContext, payment);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: paymentUrl));
        }

        [HttpGet("PaymentCallBack")]
        public async Task<IActionResult> VnPayCallback()
        {
            var vnPayResponse = _paymentService.PaymentExecute(Request.Query);
            string response;

            if (vnPayResponse.VnPayResponseCode != "00")
            {
                response = await _orderService.HandleVnPayCallback(vnPayResponse.OrderId, false);

                return Ok(BaseResponse<string>.OkResponse(response));
            }

            response = await _orderService.HandleVnPayCallback(vnPayResponse.OrderId, true);

            return Ok(BaseResponse<string>.OkResponse(response));
        }
    }
}
