using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetSpa.Core.Base;

namespace PetSpaBE.API.Controllers
{
    [ApiController]
    [Route("error")]
    public class ErrorController : ControllerBase
    {
        [HttpGet]
        [HttpDelete]
        [HttpPost]
        [HttpPut]
        public IActionResult HandleError()
        {
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = exceptionFeature?.Error;

            if (exception is ErrorException errorException)
            {
                return StatusCode(errorException.StatusCode, new
                {
                    errorCode = errorException.ErrorDetail.ErrorCode,
                    errorMessage = errorException.ErrorDetail.ErrorMessage
                });
            }

            // Xử lý các loại exception khác nếu cần
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                errorCode = "UNKNOWN_ERROR",
                errorMessage = "An unexpected error occurred."
            });
        }
    }

}
