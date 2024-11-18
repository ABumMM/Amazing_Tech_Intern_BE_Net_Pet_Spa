using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetSpa.Contract.Services.Interface;
using PetSpa.ModelViews.PackageModelViews;

namespace PetSpaBE.Razor.Pages.Package
{
    public class CreateModel : PageModel
    {
        private readonly IPackageService _packageService;

        public CreateModel(IPackageService packageService)
        {
            _packageService = packageService;
        }

        [BindProperty]
        public POSTPackageModelView Model { get; set; }

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
                // Call the package service to add a new package
                await _packageService.Add(Model);  // Gọi đến dịch vụ tạo mới package
                TempData["SuccessMessage"] = "Thêm gói dịch vụ mới thành công.";  // Success message
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Không thể thêm gói dịch vụ: {ex.Message}";  // Error message
                return Page();
            }

            return RedirectToPage("PackageList");  // Redirect to the package management page
        }
    }
}
