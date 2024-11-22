using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetSpa.Contract.Services.Interface;
using PetSpa.ModelViews.PetsModelViews;

namespace PetSpaBE.Razor.Pages.Pets
{
    public class CreateModel : PageModel
    {
        private readonly IPetService _petService;

        public CreateModel(IPetService petService)
        {
            _petService = petService;
        }

        [BindProperty]
        public POSTPetsModelView Model { get; set; } 

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
                await _petService.Add(Model); // Gọi dịch vụ thêm mới thú cưng
                TempData["SuccessMessage"] = "Thêm thú cưng mới thành công."; // Thông báo thành công
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Không thể thêm thú cưng: {ex.Message}"; // Thông báo lỗi
                return Page();
            }

            return RedirectToPage("PetList"); // Redirect về danh sách thú cưng
        }
    }
}
