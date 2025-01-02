using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.ModelViews.OrderDetailModelViews;
using PetSpa.ModelViews.ServiceModelViews;

namespace PetSpaBE.Razor.Pages.OrderDetails
{
    public class AdminOrderDetailModel : PageModel
    {
        private readonly IOrderDetailServices _orderDetailServices;
        public AdminOrderDetailModel(IOrderDetailServices OrderDetailServices)
        {
            _orderDetailServices = OrderDetailServices;
        }
        public BasePaginatedList<GETOrderDetailModelView> lst_OrderDetails { get; private set; }

        public async Task OnGetAsync(int pageNumber = 1, int pageSize = 10)
        {
            lst_OrderDetails = await _orderDetailServices.GetAll(pageNumber, pageSize);

        }
        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("ID không hợp lệ.");
            }

            try
            {
                await _orderDetailServices.Delete(id);
                TempData["SuccessMessage"] = "Dịch vụ đã được xóa thành công.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Không thể xóa dịch vụ: {ex.Message}";
            }

            // Redirect lại để tải lại danh sách
            return RedirectToPage();
        }
    }
}
