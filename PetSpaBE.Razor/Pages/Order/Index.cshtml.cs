using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.ModelViews.OrderModelViews;

namespace PetSpaBE.Razor.Pages.Order
{
    public class IndexModel : PageModel
    {
        private readonly IOrderService _orderService; 
        public GetOrderViewModel Order { get; set; }
        public List<GetOrderViewModel> Orders { get; set; } = new List<GetOrderViewModel>();
        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }
        public IndexModel(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task OnGetAsync()
        {
            try
            {
                if (!string.IsNullOrEmpty(SearchTerm))
                {
                    var order = await _orderService.GetById(SearchTerm);
                    if (order != null)
                    {
                        Order = order;
                    }
                    else
                    {
                        TempData["Error"] = "Không tìm thấy đơn hàng với ID này.";
                    }
                }
                else
                {
                    int pageNumber = 1;
                    int pageSize = 10;
                    var result = await _orderService.GetAll(pageNumber, pageSize); 
                    Orders = result.Items.ToList();
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

        public async Task<IActionResult> OnPostConfirmAsync(string id)
        {
            try
            {
                await _orderService.ConfirmOrder(id); 
                TempData["Success"] = "Đơn hàng đã được xác nhận thành công!";
            }
            catch (ErrorException ex)
            {
                TempData["Error"] = ex.Message; 
            }
            return RedirectToPage();
        }
    }
}
