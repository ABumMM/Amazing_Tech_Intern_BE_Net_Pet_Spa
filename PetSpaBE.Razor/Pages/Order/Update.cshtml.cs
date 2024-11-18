using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetSpa.ModelViews.OrderModelViews;

namespace PetSpaBE.Razor.Pages.Order
{
    public class UpdateModel : PageModel
    {
        [BindProperty]
        public PutOrderViewModel Order { get; set; } = new PutOrderViewModel();

        public string? ErrorMessage { get; set; }

        public void OnGet(string id)
        {

            if (id != null)
            {
                Order = new PutOrderViewModel
                {
                    Id = id,
                    Name = "",
                    PaymentMethod = "Credit Card",
                    LastUpdatedBy = "Admin",
                    LastUpdatedTime = DateTimeOffset.Now
                };
            }
            else
            {
                ErrorMessage = "Không tìm thấy đơn hàng.";
            }
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                return RedirectToPage("/Order/Index"); 
            }
            ErrorMessage = "Đã xảy ra lỗi khi cập nhật đơn hàng.";
            return Page();
        }
    }
}
