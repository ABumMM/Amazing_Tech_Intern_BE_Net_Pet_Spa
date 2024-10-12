using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.ModelViews.BookingModelViews;
using PetSpa.ModelViews.BookingPackageModelViews;
using PetSpa.Services.Service;
namespace PetSpaBE.API.Controllers
{
    [Authorize]
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
        public async Task<IActionResult> GetAllBookingPKs(int pageNumber, int pageSize)
        {  
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
                data: "Thêm thành công"));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingPackageById(string id)
        {
            var booking = await _bookingPackageService.GetById(id);
            return Ok(new BaseResponseModel<GETBooking_PackageVM>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: booking));
        }
    }
}
