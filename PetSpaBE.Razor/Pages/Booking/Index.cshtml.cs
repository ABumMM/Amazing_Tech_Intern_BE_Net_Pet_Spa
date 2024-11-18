using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.ModelViews.AuthModelViews;
using PetSpa.ModelViews.BookingModelViews;
using PetSpa.Services.Service;

namespace PetSpaBE.Razor.Pages.Booking
{
    public class IndexModel : PageModel
    {
        private readonly IBookingServicecs _bookingService;
        private readonly IAuthService _authService;
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
                TempData["Error"] = $"{ex.ErrorDetail.ErrorCode}: {ex.ErrorDetail.ErrorMessage}";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Đã có lỗi xảy ra. Vui lòng thử lại sau."; 
            }
           
        }
        public async Task<IActionResult> OnPostCancelAsync(string id)
        {
            try
            {
                await _bookingService.CancelBooking(id); // Gọi hàm CancelBooking từ service
                TempData["Success"] = "Booking đã được hủy thành công!";
            }
            catch (ErrorException ex)
            {
                TempData["Error"] = ex.Message; // Hiển thị lỗi nếu có
            }
            return RedirectToPage();
        }
        //public async Task<IActionResult> OnPostCancelAsync(string id)
        //{
        //    //SignInAuthModelView signInAuth = new SignInAuthModelView();
        //    //signInAuth.Email = "q1@gmailc.om";
        //    //signInAuth.Password = "Nguyenvantai2003";
        //    //await _authService.SignInAsync(signInAuth);
        //    if (string.IsNullOrEmpty(id))
        //    {
        //        TempData["Error"] = "ID không được để trống.";
        //        return Page();
        //    }
        //    SignInAuthModelView signInAuth = new SignInAuthModelView
        //    {
        //        Email = "q1@gmailc.om",
        //        Password = "Nguyenvantai2003"
        //    };
        //    var user = await _authService.SignInAsync(signInAuth);

        //    if (user == null)
        //    {
        //        TempData["Error"] = "Đăng nhập thất bại.";
        //        return Page();
        //    }

        //    try
        //    {
        //        // Lấy UserId sau khi đăng nhập thành công
        //        var currentUserId = user;

        //        // Gọi hàm CancelBooking với UserId để cập nhật trường CreatedBy
        //        await _bookingService.CancelBooking(id);

        //        TempData["Message"] = "Hủy booking thành công.";
        //    }
        //    catch (ErrorException ex)
        //    {
        //        TempData["Error"] = $"{ex.ErrorDetail.ErrorCode}: {ex.ErrorDetail.ErrorMessage}";
        //    }
        //    catch (Exception)
        //    {
        //        TempData["Error"] = "Đã có lỗi xảy ra khi hủy booking.";
        //    }

        //    return RedirectToPage();
        //}
    }
}
