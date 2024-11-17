using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.ModelViews.BookingModelViews;
using PetSpa.Services.Service;

namespace PetSpaBE.Razor.Pages.Booking
{
    public class IndexModel : PageModel
    {
        private readonly IBookingServicecs _bookingService;
        public GETBookingVM Booking { get; set; }
        public List<GETBookingVM> Bookings { get; set; } = new List<GETBookingVM>();
        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }
        public IndexModel(IBookingServicecs bookingService)
        {
            _bookingService = bookingService;
        }
        public async Task OnGetAsync()
        {
            try
            {
                if (!string.IsNullOrEmpty(SearchTerm))
                {
                    var booking = await _bookingService.GetById(SearchTerm);
                    if (booking != null)
                    {
                        Booking = booking;
                    }
                    else
                    {
                        TempData["Error"] = "Không tìm thấy booking với ID này.";
                    }
                }
                else
                {
                    int pageNumber = 1;
                    int pageSize = 10;
                    var result = await _bookingService.GetAll(pageNumber, pageSize);
                    Bookings = result.Items.ToList();
                }
            }
            catch (ErrorException ex)
            {
                // Lưu thông báo lỗi chi tiết vào TempData
                TempData["Error"] = $"{ex.ErrorDetail.ErrorCode}: {ex.ErrorDetail.ErrorMessage}";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Đã có lỗi xảy ra. Vui lòng thử lại sau."; // Lỗi chung
            }
        }
        //public async Task OnGetByIdAsync(string id)
        //{
        //    if (!string.IsNullOrEmpty(id))
        //    {
        //        // Gọi GetById nếu có ID trong query string
        //        var booking = await _bookingService.GetById(id);
        //        Booking = booking; // Gán kết quả vào thuộc tính Booking
        //    }
        //    else
        //    {
        //        var result = await _bookingService.GetAll(1,10);
        //        Bookings = result.Items.ToList();
        //    }
        //}
    }
}
