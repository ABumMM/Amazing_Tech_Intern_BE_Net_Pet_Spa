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

namespace PetSpaBE.API.Controllers
{
    [Authorize]
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
        public async Task<IActionResult> GetAllBookings(int pageNumber, int pageSize)
        {
            var bookings = await _bookingService.GetAll(pageNumber, pageSize);
            return Ok(new BaseResponseModel<BasePaginatedList<GETBookingVM>>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: bookings));

        }

        [HttpGet("ByUserId")]
        public async Task<IActionResult> GetAllBookingByCustomer(int pageNumber, int pageSize)
        {
            var bookings = await _bookingService.GetAllBookingByCustomer(pageNumber, pageSize);
            return Ok(new BaseResponseModel<BasePaginatedList<GETBookingVM>>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: bookings));

        }

        [HttpPost]
        public async Task<IActionResult> AddBooking(POSTBookingVM bookingVM)
        {
            await _bookingService.Add(bookingVM);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Add Booking successful"));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(string id)
        {
            var booking = await _bookingService.GetById(id);
            return Ok(new BaseResponseModel<GETBookingVM>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: booking));
        }

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
