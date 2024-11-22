using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.ModelViews.AuthModelViews;
using PetSpa.ModelViews.PackageModelViews;
using PetSpa.Services.Service;

namespace PetSpaBE.Razor.Pages.Package
{
    public class AdminPackageModel : PageModel
    {
        private readonly IPackageService _packageService;
        public BasePaginatedList<GETPackageModelView> lst_package { get; private set; }
        private readonly IAuthService _authService;

        public AdminPackageModel(IPackageService packageService, IAuthService authService)
        {
            _packageService = packageService;
            _authService = authService;
        }

        public async Task OnGet(int pageNumber)
        {
            if (pageNumber == 0)
            {
                pageNumber = 1;
            }
            const int size = 6;

            lst_package = await _packageService.GetAll(pageNumber, size);
        }

        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            SignInAuthModelView signInAuth = new SignInAuthModelView
            {
                Email = "HungCus123@gmail.com",
                Password = "Hungapa123@"
            };

            await _authService.SignInAsync(signInAuth);

            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("ID không hợp lệ.");
            }

            try
            {
                await _packageService.Delete(id);
                TempData["SuccessMessage"] = "Gói đã được xóa thành công.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Không thể xóa gói: {ex.Message}";
            }

            // Redirect lại để tải lại danh sách
            return RedirectToPage();
        }
    }
}
