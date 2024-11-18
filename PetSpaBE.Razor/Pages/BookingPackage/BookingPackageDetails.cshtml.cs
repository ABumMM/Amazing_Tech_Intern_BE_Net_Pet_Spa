using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetSpa.Contract.Services.Interface;
using PetSpa.ModelViews.BookingPackageModelViews;

namespace PetSpaBE.Razor.Pages.BookingPackage
{
    public class BookingPackageDetailsModel : PageModel
    {
        private readonly IBookingPackage_Service _bookingPackageService;

        public BookingPackageDetailsModel(IBookingPackage_Service bookingPackageService)
        {
            _bookingPackageService = bookingPackageService;
        }

        public GETBooking_PackageVM BookingPackage { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            // G?i service ?? l?y thông tin chi ti?t c?a BookingPackage
            BookingPackage = await _bookingPackageService.GetById(id);

            if (BookingPackage == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
