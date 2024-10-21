using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.ModelViews.BookingModelViews;
using PetSpa.ModelViews.ModelViews;
using PetSpa.ModelViews.PackageModelViews;
using PetSpa.Services.Service;
using Swashbuckle.AspNetCore.Annotations;

namespace PetSpaBE.API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingServicecs _bookingService;

        public BookingController(IBookingServicecs bookingService)
        {
            _bookingService = bookingService;
        }
        [HttpGet]
        [SwaggerOperation(
            Summary = "Authorization: Admin",
            Description = "GetAll Booking"
            )]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllBookings(int pageNumber, int pageSize)
        {
            var bookings = await _bookingService.GetAll(pageNumber, pageSize);
            return Ok(new BaseResponseModel<BasePaginatedList<GETBookingVM>>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: bookings));
        }
        [HttpGet("ByUserId")]
        [SwaggerOperation(
            Summary = "Authorization: Anyone",
            Description = "Get Booking ByUserId )"
            )]
        public async Task<IActionResult> GetAllBookingByCustomer(int pageNumber, int pageSize)
        {
            var bookings = await _bookingService.GetAllBookingByCustomer(pageNumber, pageSize);
            return Ok(new BaseResponseModel<BasePaginatedList<GETBookingVM>>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: bookings));

        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Authorization: Anyone",
            Description = "Post Booking )"
            )]
        public async Task<IActionResult> AddBooking(POSTBookingVM bookingVM)
        {
            await _bookingService.Add(bookingVM);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Add Booking successful"));
        }
        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Authorization: Anyone",
            Description = "GetBookingById )"
            )]
        public async Task<IActionResult> GetBookingById(string id)
        {
            var booking = await _bookingService.GetById(id);
            return Ok(new BaseResponseModel<GETBookingVM>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: booking));
        }
        [SwaggerOperation(
            Summary = "Authorization: Anyone",
            Description = "Update Booking )"
            )]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBooking(string id, [FromBody] POSTBookingVM bookingVM)
        {
            await _bookingService.Update(bookingVM, id);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Update booking success"));
        }
        [HttpPut("{id}/cancel")]
        [SwaggerOperation(
            Summary = "Authorization: Anyone",
            Description = "Cancel Booking )"
            )]
        public async Task<IActionResult> CancelBooking(string id)
        {
            await _bookingService.CancelBooking(id);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Hủy booking thành công"));
        }

    }
}
