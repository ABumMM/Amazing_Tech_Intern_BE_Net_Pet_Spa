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
                await _packageService.Add(Model);  
                TempData["SuccessMessage"] = "Thêm gói dịch vụ mới thành công.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Không thể thêm gói dịch vụ: {ex.Message}";  
                return Page();
            }

            return RedirectToPage("./AdminPackage"); 
        }
    }
}
