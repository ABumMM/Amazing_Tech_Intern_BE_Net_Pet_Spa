using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.ModelViews.ServiceModelViews;

namespace PetSpaBE.Razor.Pages.Services
{
    public class AdminServicesModel : PageModel
    {
        private readonly IServicesService _serviceService;
        public AdminServicesModel(IServicesService ServicesService)
        {
            _serviceService = ServicesService;
        }
        public BasePaginatedList<ServiceResposeModel> lst_Services { get; private set; }

        public async Task OnGetAsync(int pageNumber = 1, int pageSize = 10)
        {
            lst_Services = await _serviceService.GetAll(pageNumber, pageSize);
               
        }
        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("ID không hợp lệ.");
            }

            try
            {
                await _serviceService.Delete(id);
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
