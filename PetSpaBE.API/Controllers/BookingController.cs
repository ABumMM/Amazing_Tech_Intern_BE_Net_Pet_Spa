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
        public async Task<IActionResult> GetAllBookings(int pageNumber = 1, int pageSize = 2)
        {
            var bookings = await _bookingService.GetAll(pageNumber, pageSize);
            return Ok(new BaseResponseModel<BasePaginatedList<GETBookingVM>>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: bookings));

        }
        [HttpPost]
        public async Task<IActionResult> AddBooking(POSTBookingVM bookingVM)
        {
            await _bookingService.Add(bookingVM);
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(string id)
        {
            var booking = await _bookingService.GetById(id);

            if (booking == null)
            {
                // Nếu không tìm thấy booking, trả về NotFound
                return NotFound(new
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    ErrorCode = "BookingNotFound",
                    Message = "Không tìm thấy Booking với ID đó."
                });
            }
            
            return Ok(new BaseResponseModel<GETBookingVM>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: booking));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBooking(string id, [FromBody] POSTBookingVM bookingVM)
        {
            if (bookingVM == null)
            {
                return NotFound(new
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    ErrorCode = "InValid booking data",
                    Message = "Booking không có dữ liệu."
                });
            }

            var existingBooking = await _bookingService.GetById(id);

            if (existingBooking == null)
            {
                return NotFound(new
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    ErrorCode = "BookingNotFound",
                    Message = "Không tìm thấy."
                });
            }

            // Gọi phương thức update từ _bookingService và cập nhật thông tin
            await _bookingService.Update(bookingVM, id);
            

            return Ok("Booking updated successfully!");
        }

    }
}
