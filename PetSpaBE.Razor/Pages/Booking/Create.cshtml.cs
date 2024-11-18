using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetSpa.Contract.Services.Interface;
using PetSpa.ModelViews.BookingModelViews;
using PetSpa.ModelViews.BookingPackageModelViews;
using PetSpa.ModelViews.UserModelViews;
using PetSpa.Services.Service;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.ModelViews.AuthModelViews;

namespace PetSpaBE.Razor.Pages.Booking
{
    public class CreateModel : PageModel
    {
        public readonly IBookingServicecs _bookingService;
        public readonly IUserService _userService;
        public readonly IAuthService _authService;
        [BindProperty]
        public List<GETUserModelView> Employees { get; set; }
        [BindProperty]
        public POSTBookingVM BookingVM { get; set; }

        [BindProperty]
        public string SelectedEmployeeId { get; set; }
        public string ErrorMessage { get; set; }
        public CreateModel(IBookingServicecs bookingService, IUserService userService, IAuthService authService)
        {
            _bookingService = bookingService;
            _userService = userService;
            _authService = authService;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            if (BookingVM == null)
            {
                BookingVM = new POSTBookingVM
                {
                    Description = string.Empty,
                    Date = DateTime.Now,
                    ApplicationUserId = string.Empty,

                    Status = string.Empty

                };
            }
            var result = await _userService.GetEmployees(1, 100);
            Employees = result.Items.ToList();

            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {

                
                if (BookingVM == null)
                {
                    throw new InvalidOperationException("Booking không th? null.");
                }
                BookingVM.ApplicationUserId = "EE82A3D9-EC37-449B-A5AD-08DCF105E5AA";
                await _bookingService.Add(BookingVM);
                return RedirectToPage("/Booking/Index");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                //return Page();
            }
            return Page();
        }
    
    }
}
