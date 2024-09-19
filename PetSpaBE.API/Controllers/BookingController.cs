using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.ModelViews.ModelViews;
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
            
            IList<Bookings> bookings = await _bookingService.GetAll();

            int totalPackage = bookings.Count;
            // Thực hiện phân trang
            var paginatedPackages = bookings
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Tạo đối tượng BasePaginatedList để trả về
            var paginatedList = new BasePaginatedList<Bookings>(paginatedPackages, totalPackage, pageNumber, pageSize);

            return Ok(paginatedList);
        }
        [HttpPost]
        public async Task<IActionResult> AddBooking(BookingResponseModel bookingVM)
        {
            await _bookingService.Add(bookingVM);
            return Ok();
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteBooking(string id)
        {
            try
            {
                await _bookingService.Delete(id);
                return Ok();
            }
            catch
            {
                return NotFound("Booking not found!");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(string id)
        {
            var bookings = await _bookingService.GetById(id) ?? null;
            if (bookings is null)
                return NotFound("Booking not found!");
            return Ok(bookings);
        }
        
        [HttpPut]
        public async Task<IActionResult> UpdateBooking(Bookings booking)
        {
            try
            {
                await _bookingService.Update(booking);
                return Ok();
            }
            catch
            {
                return NotFound("Booking not found!");
            }
        }
    }
}
