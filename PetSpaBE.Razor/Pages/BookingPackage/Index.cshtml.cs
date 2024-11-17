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
        public string SearchTerm { get; set; }
        public BasePaginatedList<GETBooking_PackageVM> BookingPackages { get; set; }
        public GETBooking_PackageVM BookingPackage { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public async Task OnGetAsync()
        {
            try
            {
                if (!string.IsNullOrEmpty(SearchTerm))
                {
                    await SearchBookingPackageById(SearchTerm);
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
        private async Task SearchBookingPackageById(string searchTerm)
        {
            searchTerm = "7c373b798b024afa84ca507a7f469418"; // Hardcoded for testing
            
            BookingPackage = await _bookingPackageService.GetById(searchTerm);
            if (BookingPackage != null)
            {
                // Nếu tìm thấy BookingPackage với ID cụ thể, chỉ hiển thị một kết quả.
                BookingPackages = new BasePaginatedList<GETBooking_PackageVM>(new List<GETBooking_PackageVM> { BookingPackage }, 1, 1, 1);
            }
            else
            {
                TempData["Error"] = "Không tìm thấy BookingPackage với ID này.";
                BookingPackages = null; // Không hiển thị danh sách nếu không tìm thấy.
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
