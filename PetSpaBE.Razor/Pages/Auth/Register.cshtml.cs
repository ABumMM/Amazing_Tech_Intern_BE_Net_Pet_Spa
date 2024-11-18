using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetSpa.ModelViews.AuthModelViews;
using PetSpa.Services.Service;

namespace PetSpaBE.Razor.Pages.Auth
{
    public class RegisterModel : PageModel
    {
        private readonly AuthService _authService;

        public RegisterModel(AuthService authService)
        {
            _authService = authService;
        }

        [BindProperty]
        public SignUpAuthModelView SignUpModel { get; set; }

        [BindProperty]
        public string RoleName { get; set; }

        public string Message { get; set; }

        public void OnGet()
        {
            // Trường hợp khi chưa đăng nhập
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Message = "Please check the form for errors.";
                return Page();
            }

            try
            {
                // Gọi AuthService để xử lý đăng ký
                var token = await _authService.SignUpAsync(SignUpModel, RoleName);
                Message = "Registration successful. Please log in.";

                // Redirect đến trang đăng nhập hoặc trang chủ sau khi đăng ký thành công
                return RedirectToPage("/Auth/Login");
            }
            catch (Exception ex)
            {
                Message = $"Error during registration: {ex.Message}";
                return Page();
            }
        }
    }
}
