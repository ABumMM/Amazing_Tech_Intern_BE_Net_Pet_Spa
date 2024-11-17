using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetSpa.Contract.Services.Interface;
using PetSpa.ModelViews.BookingModelViews;
using PetSpa.ModelViews.UserModelViews;
using PetSpa.Services.Service;
using System.Security.Claims;

namespace PetSpaBE.Razor.Pages.Booking
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly IBookingServicecs _bookingService;
        private readonly IUserService _userService;
        [BindProperty]
        public POSTBookingVM Booking { get; set; } 

        // Danh sách nhân viên để hiển thị trong ComboBox
        public List<GETUserModelView> Employees { get; set; } = new List<GETUserModelView>();

        public CreateModel(IBookingServicecs bookingService, IUserService userService)
        {
            _bookingService = bookingService;
            _userService = userService;
        }

        //public async Task<IActionResult> OnGetAsync()
        //{
        //    // Gọi phương thức GetEmployees để lấy danh sách nhân viên
        //    var employeeList = await _userService.GetEmployees(1, 100); // Lấy 100 nhân viên đầu tiên (có thể thay đổi)
        //    Employees = employeeList.Items.ToList();

        //    // Đảm bảo ApplicationUserId có giá trị mặc định để tránh lỗi required
        //    if (!Employees.Any())
        //    {
        //        ModelState.AddModelError(string.Empty, "Không có nhân viên nào được tìm thấy.");
        //        return Page();
        //    }

        //    // Gán ApplicationUserId mặc định từ danh sách nhân viên nếu có
        //    Booking.ApplicationUserId = Employees.First().Id;

        //    return Page();
        //}

        //public async Task<IActionResult> OnPostAsync()
        //{
        //    var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    if (string.IsNullOrEmpty(currentUserId))
        //    {
        //        ModelState.AddModelError(string.Empty, "Không tìm thấy thông tin người dùng đăng nhập.");
        //        return Page();
        //    }

        //    // Gán User ID của người tạo vào thuộc tính CreatedBy
        //    Booking.CreatedBy = currentUserId;

        //    if (!ModelState.IsValid)
        //    {
        //        return Page();
        //    }

        //    try
        //    {
        //        // Gửi thông tin booking đến service để thêm mới
        //        await _bookingService.Add(Booking);
        //        return RedirectToPage("/Booking/Index");
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError(string.Empty, $"Có lỗi xảy ra: {ex.Message}");
        //        return Page();
        //    }
        //}
    }
}
