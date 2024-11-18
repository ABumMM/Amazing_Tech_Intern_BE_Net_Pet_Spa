using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetSpa.Contract.Services.Interface;
using PetSpa.ModelViews.OrderModelViews;
using PetSpa.Services.Service;
using System;
using System.Threading.Tasks;

namespace PetSpaBE.Razor.Pages.Order
{
    public class CreateModel : PageModel
    {
        public readonly IOrderService _orderService;
        public readonly IUserService _userService;

        [BindProperty]
        public PostOrderViewModel OrderVM { get; set; } 

        [BindProperty]
        public string SelectedCustomerId { get; set; } 
        public string ErrorMessage { get; set; }

        public CreateModel(IOrderService orderService, IUserService userService)
        {
            _orderService = orderService;
            _userService = userService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (OrderVM == null)
            {
                // Khởi tạo OrderVM nếu nó chưa có
                OrderVM = new PostOrderViewModel
                {
                    Name = string.Empty,
                    PaymentMethod = "Unknown",
                    CustomerID = string.Empty,
                    BookingID = string.Empty,
                    CreatedTime = DateTimeOffset.Now,
                    CreatedBy = string.Empty
                };
            }

            var result = await _userService.GetCustomers(1, 100); // Giả sử GetCustomers trả về danh sách khách hàng
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (OrderVM == null)
                {
                    throw new InvalidOperationException("Đơn hàng không thể là null.");
                }

                await _orderService.Add(OrderVM); 

                return RedirectToPage("/Order/Index"); // Chuyển hướng sau khi tạo đơn hàng thành công
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page(); // Trả lại trang với thông báo lỗi
            }
        }
    }
}
