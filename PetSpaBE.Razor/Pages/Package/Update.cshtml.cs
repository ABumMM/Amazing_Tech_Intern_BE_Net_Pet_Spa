using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Services.Interface;
using PetSpa.ModelViews.PackageModelViews;
using PetSpa.ModelViews.RankModelViews;
using PetSpa.Services.Service;

namespace PetSpaBE.Razor.Pages.Package
{
    public class UpdateModel : PageModel
    {
        private readonly IPackageService _packageService;
        public GETPackageModelView packageModelView { get; set; }
        public UpdateModel(IPackageService packageService) {
            _packageService = packageService;
        }
        public async Task<IActionResult> OnGet(string id)
        {
          
            packageModelView = await _packageService.GetById(id); 
            if (packageModelView == null)
            {
                return NotFound();
            }
            return Page();
        }
        [BindProperty]
        public PUTPackageModelView UpdatePackage { get; set; }
        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (!ModelState.IsValid)
            {
                return Page(); // Trả về form cùng với lỗi nếu có.
            }

            await _packageService.Update(UpdatePackage);
            // Redirect về trang danh sách hoặc chi tiết
            return RedirectToPage("./AdminPackage"); // Thay đổi "./Index" nếu cần.
        }
    }
}
