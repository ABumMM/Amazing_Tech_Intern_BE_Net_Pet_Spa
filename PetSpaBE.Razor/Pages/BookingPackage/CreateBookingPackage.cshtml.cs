using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.ModelViews.BookingPackageModelViews;
using PetSpa.Services.Service;

namespace PetSpaBE.Razor.Pages.BookingPackage
{
    public class CreateBookingPackageModel : PageModel
    {
        private readonly IBookingPackage_Service _bookingPackageService;
        public string ErrorMessage { get; set; }
        public CreateBookingPackageModel(IBookingPackage_Service bookingPackageService)
        {
            _bookingPackageService = bookingPackageService;
        }

        [BindProperty]
        public Booking_PackageVM BookingPackageVM { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (BookingPackageVM == null)
            {
                BookingPackageVM = new Booking_PackageVM
                {
                    BookingId = string.Empty, 
                    PackageId = string.Empty,
                  
                    
                };
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                if (BookingPackageVM == null)
                {
                    throw new InvalidOperationException("BookingPackageVM không th? null.");
                }
                await _bookingPackageService.Add(BookingPackageVM);
                return RedirectToPage("/BookingPackage/Index");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }
        }
    }
}
