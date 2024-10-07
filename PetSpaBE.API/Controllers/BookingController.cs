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
            if(pageSize < 1)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "PageSize không hợp lệ!");
            }
            var bookings = await _bookingService.GetAll(pageNumber, pageSize);
            return Ok(new BaseResponseModel<BasePaginatedList<GETBookingVM>>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: bookings));

        }


        //[HttpPost]
        //public async Task<IActionResult> AddBooking(POSTBookingVM bookingVM)
        //{
        //    var isSuccess = await _bookingService.Add(bookingVM);

        //    if (!isSuccess)
        //    {
        //        return Ok(new BaseResponseModel<string>(
        //        statusCode: StatusCodes.Status404NotFound,
        //        code: ResponseCodeConstants.SUCCESS,
        //        data: "OrderId không hợp lệ"));

        //    }

        //    return Ok(new BaseResponseModel<string>(
        //        statusCode: StatusCodes.Status200OK,
        //        code: ResponseCodeConstants.SUCCESS,
        //        data: "Thêm thành công"));
        //}
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

            if (booking == null)
            {
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
            await _bookingService.Update(bookingVM, id);
            

            return Ok("Booking updated successfully!");
        }

    }
}
