using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;

namespace PetSpaBE.Razor.Pages.Booking
{
    public class CancelModel : PageModel
    {
        private readonly IBookingServicecs _bookingService;

        [BindProperty]
        public string BookingId { get; set; }

        public CancelModel(IBookingServicecs bookingService)
        {
            _bookingService = bookingService;
        }

        public async Task<IActionResult> OnPostCancelAsync()
        {
            if (string.IsNullOrWhiteSpace(BookingId))
            {
                ModelState.AddModelError(string.Empty, "Id không ???c ?? tr?ng.");
                return Page();
            }

            try
            {
                await _bookingService.CancelBooking(BookingId);
                TempData["Message"] = "H?y booking thành công.";
            }
            catch (ErrorException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                TempData["Error"] = ex.Message;
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Có l?i x?y ra khi h?y booking.");
                TempData["Error"] = "Có l?i x?y ra khi h?y booking.";
            }

            return RedirectToPage();
        }
    }
}
