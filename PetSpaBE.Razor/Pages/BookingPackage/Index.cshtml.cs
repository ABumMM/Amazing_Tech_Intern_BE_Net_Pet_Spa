using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.ModelViews.BookingModelViews;
using PetSpa.ModelViews.BookingPackageModelViews;
using PetSpa.Services.Service;

namespace PetSpaBE.Razor.Pages.BookingPackage
{
    public class IndexModel : PageModel
    {
        private readonly IBookingPackage_Service _bookingPackageService;

        public IndexModel(IBookingPackage_Service bookingPackageService)
        {
            _bookingPackageService = bookingPackageService;
        }
        
        public BasePaginatedList<GETBooking_PackageVM> BookingPackages { get; set; }
       // public List<GETBooking_PackageVM> BookingPKs { get; set; } = new List<GETBooking_PackageVM>();
        public GETBooking_PackageVM BookingPackage { get; set; }
        public string SearchTerm { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public async Task OnGetAsync()
        {
            try
            {
                if (!string.IsNullOrEmpty(SearchTerm))
                {
                    var booking = await _bookingPackageService.GetById(SearchTerm);
                    if (booking != null)
                    {
                        BookingPackage = booking;
                    }
                    else
                    {
                        TempData["Error"] = "Không tìm thấy booking với ID này.";
                    }
                }
                else
                {
                    BookingPackages = await _bookingPackageService.GetAll(PageNumber, PageSize);
                }
            }
            catch (ErrorException ex)
            {
                TempData["Error"] = $"{ex.ErrorDetail.ErrorCode}: {ex.ErrorDetail.ErrorMessage}";
            }
            catch (Exception)
            {
                TempData["Error"] = "Đã có lỗi xảy ra. Vui lòng thử lại sau.";
            }
        }
        public async Task<IActionResult> OnGetDetailsAsync(string id)
        {
            BookingPackage = await _bookingPackageService.GetById(id);

            if (BookingPackage == null)
            {
                return NotFound();
            }

            return Page();
        }

    }
}
