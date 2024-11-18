using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetSpa.Contract.Services.Interface;
using PetSpa.ModelViews.OrderDetailModelViews;

namespace PetSpaBE.Razor.Pages.OrderDetails
{
    public class CreateOrderDetailModel : PageModel
    {
        private readonly IOrderDetailServices _orderDetailServices;

        public CreateOrderDetailModel(IOrderDetailServices orderDetailServices)
        {
            _orderDetailServices = orderDetailServices;
        }

        [BindProperty]
        public POSTOrderDetailModelView Model { get; set; }

        public IActionResult OnGet()
        {
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


                await _orderDetailServices.Add(Model);  // Gọi đến dịch vụ tạo mới
                TempData["SuccessMessage"] = "Thêm chi tiết mới thành công.";  // Thông báo thành công
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Không thể thêm chi tiết: {ex.Message}";  // Thông báo lỗi
                return Page();
            }

            return RedirectToPage("ServiceList");  // Redirect về trang quản lý sản phẩm
        }
    }
}
