using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetSpa.Contract.Services.Interface;
using PetSpa.ModelViews.BookingModelViews;
using PetSpa.ModelViews.BookingPackageModelViews;
using PetSpa.ModelViews.UserModelViews;
using PetSpa.Services.Service;
using PetSpa.Contract.Repositories.Entity;

namespace PetSpaBE.Razor.Pages.Booking
{
    public class CreateModel : PageModel
    {
        public readonly IBookingServicecs _bookingService;
        public readonly IUserService _userService;
        [BindProperty]
        public List<GETUserModelView> Employees { get; set; }
        [BindProperty]
        public POSTBookingVM BookingVM { get; set; }
        [BindProperty]
        public string SelectedEmployeeId { get; set; }
        public string ErrorMessage { get; set; }
        public CreateModel(IBookingServicecs bookingService, IUserService userService)
        {
            _bookingService = bookingService;
            _userService = userService;
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
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            //if (string.IsNullOrEmpty(SelectedEmployeeId) || !Guid.TryParse(SelectedEmployeeId, out var employeeId))
            //{
            //    ErrorMessage = "Vui lòng ch?n nhân viên h?p l?.";
            //    return Page();
            //}

            //var bookingVM = new POSTBookingVM
            //{
            //    Description = BookingVM.Description,
            //    Status = BookingVM.Status,
            //    Date = BookingVM.Date,
            //    ApplicationUserId = SelectedEmployeeId
            //};

            //try
            //{
            //    await _bookingService.Add(bookingVM);
            //}
            //catch (Exception ex)
            //{
            //    ErrorMessage = $"L?i khi t?o booking: {ex.Message}";
            //    return Page();
            //}

            //return RedirectToPage("Index");
            

            try
            {
                if (BookingVM == null)
                {
                    throw new InvalidOperationException("Booking không th? null.");
                }
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
