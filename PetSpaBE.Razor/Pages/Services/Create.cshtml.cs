using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetSpa.Contract.Services.Interface;
using PetSpa.ModelViews.ServiceModelViews;

namespace PetSpaBE.Razor.Pages.Services
{
    public class CreateModel : PageModel
    {
        
            private readonly IServicesService _serviceService;

            public CreateModel(IServicesService serviceService)
            {
                _serviceService = serviceService;
            }

            [BindProperty]
            public ServiceCreateModel Model { get; set; }

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


                    await _serviceService.Add(Model);  // Gọi đến dịch vụ tạo mới
                    TempData["SuccessMessage"] = "Thêm sản phẩm mới thành công.";  // Thông báo thành công
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Không thể thêm sản phẩm: {ex.Message}";  // Thông báo lỗi
                    return Page();
                }

                return RedirectToPage("ServiceList");  // Redirect về trang quản lý sản phẩm
            }
    }
    
}
