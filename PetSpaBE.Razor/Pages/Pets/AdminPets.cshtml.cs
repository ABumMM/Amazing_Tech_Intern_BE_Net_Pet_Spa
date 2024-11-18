using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.ModelViews.PetsModelViews;
using PetSpa.ModelViews.ServiceModelViews;
using PetSpa.Services.Service;

namespace PetSpaBE.Razor.Pages.Pets
{
    public class AdminPetsModel : PageModel
    {
        private readonly IPetService _petService;

        public AdminPetsModel(IPetService petService)
        {
            _petService = petService;
        }

        public BasePaginatedList<GETPetsModelView> lst_Pets { get; private set; }

        public async Task OnGet(int pageNumber)
        {

            if (pageNumber == 0)
            {
                pageNumber = 1;
            }
            const int size = 6;

            lst_Pets = await _petService.GetAll(pageNumber, size);
        }
       
        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            if (id == string.Empty)
            {
                return BadRequest("ID không hợp lệ.");
            }

            try
            {
                await _petService.Delete(id);
                TempData["SuccessMessage"] = "Thú cưng đã được xóa thành công.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Không thể xóa thú cưng: {ex.Message}";
            }

            return RedirectToPage();
        }
    }
}
