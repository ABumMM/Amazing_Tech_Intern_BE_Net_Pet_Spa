using Azure;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using PetSpa.ModelViews.AuthModelViews;

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
                ErrorMessage = "Invalid email or password.";
                return Page();
            }

            var options = new CookieOptions
            {
                Expires = DateTime.Now.AddHours(1),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            };

            Response.Cookies.Append("AuthToken", token, options);

            var userRoles = await _authService.GetUserRolesAsync(Email);

            if (userRoles.Contains("Customer"))
            {
                return RedirectToPage("/Index");
            }
            else if (userRoles.Contains("Admin") || userRoles.Contains("Employee"))
            {
                return RedirectToPage("/Admin/Index");
            }

            ErrorMessage = "Account not found. Please Signup or contact support.";
            return Page();
        }
    }
}
