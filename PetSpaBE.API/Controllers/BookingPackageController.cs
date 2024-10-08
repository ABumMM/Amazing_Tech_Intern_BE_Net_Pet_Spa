using Microsoft.AspNetCore.Mvc;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.ModelViews.BookingPackageModelViews;
namespace PetSpaBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingPackageController : ControllerBase
    {
        private readonly IBookingPackage_Service _bookingPackageService;
        public BookingPackageController(IBookingPackage_Service bookingPKService)
        {
            _bookingPackageService = bookingPKService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllBookingPKs(int pageNumber = 1, int pageSize = 2)
        {
            if (pageSize < 1)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "PageSize không hợp lệ!");
            }
            var bookingPKs = await _bookingPackageService.GetAll(pageNumber, pageSize);
            return Ok(new BaseResponseModel<BasePaginatedList<GETBooking_PackageVM>>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: bookingPKs));
        }
        [HttpPost]
        public async Task<IActionResult> AddBookingPK(Booking_PackageVM bookingPKVM)
        {
            await _bookingPackageService.Add(bookingPKVM);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Them thanh cong"));
        }
        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetBookingPKById(string id)
        //{
        //    var bookingPK = await _bookingPackageService.GetById(id);

        //    if (bookingPK == null)
        //    {
        //        return NotFound(new
        //        {
        //            StatusCode = StatusCodes.Status404NotFound,
        //            ErrorCode = "Booking_PackageNotFound",
        //            Message = "Không tìm thấy Booking với ID đó."
        //        });
        //    }
        //    return Ok(new BaseResponseModel<List<Booking_PackageVM>>(
        //        statusCode: StatusCodes.Status200OK,
        //        code: ResponseCodeConstants.SUCCESS,
        //        data: bookingPK));
        //}
        [HttpDelete("{bookingId}/{packageId}")]
        public async Task<IActionResult> DeleteBookingPackage(string bookingId, string packageId)
        {
            var result = await _bookingPackageService.DeleteBookingPackageAsync(bookingId, packageId);

            if (!result)
            {
                return NotFound(new
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    ErrorCode = "BookingPackageNotFound",
                    Message = "Không tìm thấy BookingPackage để xóa."
                });
            }
            return Ok(new
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Xóa BookingPackage thành công."
            });
        }
    }
}
