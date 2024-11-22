using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetSpa.ModelViews.AuthModelViews;
using PetSpa.Services.Service;

namespace PetSpaBE.Razor.Pages.Auth
{
    public class LoginModel : PageModel
    {
        private readonly IAuthService _authService;

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public LoginModel(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ErrorMessage = "Invalid input. Please check your email and password.";
                return Page();
            }

            var token = await _authService.SignInAsync(new SignInAuthModelView
            {
                Email = Email,
                Password = Password
            });

            if (token == null)
            {
                // Đăng nhập thất bại
                ErrorMessage = "Invalid email or password.";
                return Page();
            }
            HttpContext.Session.SetString("jwtToken", token);
            Response.Cookies.Append("AuthToken", token);

            var userRoles = await _authService.GetUserRolesAsync(Email);

            if (userRoles.Contains("Customer"))
            {
                return RedirectToPage("/Index");
            }
            else if (userRoles.Contains("Admin") || userRoles.Contains("Employee"))
            {
                return RedirectToPage("/HomeAdmin");
            }

            ErrorMessage = "Account not found. Please Signup or contact support.";
            return Page();
        }

    }
}
