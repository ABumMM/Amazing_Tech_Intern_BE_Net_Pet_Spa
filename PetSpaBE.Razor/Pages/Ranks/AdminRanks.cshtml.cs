using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.ModelViews.AuthModelViews;
using PetSpa.ModelViews.RankModelViews;
using PetSpa.Services.Service;

namespace PetSpaBE.Razor.Pages.Ranks
{
    public class AdminRanksModel : PageModel
    {
        private readonly IRankService _rankService;
        public BasePaginatedList<GetRankViewModel> lst_rank { get; private set; }
        private readonly IAuthService _authService;
        public AdminRanksModel(IRankService rankSerivce, IAuthService authService)
        {
            _rankService = rankSerivce;
            _authService = authService;
        }
        public async Task OnGet( int pageNumber) {

            if (pageNumber == 0) { 
                pageNumber = 1;
            }
            const int size = 6;
            
            lst_rank = await _rankService.GetAll(pageNumber,size);
        }
        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            SignInAuthModelView signInAuth = new SignInAuthModelView();
            signInAuth.Email = "admin@gmail.com";
            signInAuth.Password = "Admin@123";
            await _authService.SignInAsync(signInAuth);
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("ID không hợp lệ.");
            }

            try
            {
                await _rankService.Delete(id);
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
