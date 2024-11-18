using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.ModelViews.BookingModelViews;
using PetSpa.ModelViews.ServiceModelViews;
using PetSpa.ModelViews.UserModelViews;
using PetSpa.Services.Service;

namespace PetSpaBE.Razor.Pages.Booking
{
    public class UpdateModel : PageModel
    {
        private readonly IBookingServicecs _bookingService;
        public readonly IUserService _userService;
        public UpdateModel(IBookingServicecs bookingService, IUserService userService)
        {
            _bookingService = bookingService;
            _userService = userService;
        }
        [BindProperty]
        public List<GETUserModelView> Employees { get; set; }
        [BindProperty]
        public string SelectedEmployeeId { get; set; }
        public string ErrorMessage { get; set; }
        [BindProperty]
        public POSTBookingVM Booking { get; set; }
        public string BookingId { get; set; }
        public async Task<IActionResult> OnGetAsync(string id)
        {
            var booking = await _bookingService.GetById(id); // Ensure you have a method to get the service by ID
            if (booking == null)
            {
                return NotFound();
            }

            Booking = new POSTBookingVM
            {
                Description = booking.Description,
                Status = booking.Status,
                Date = booking.Date,
                ApplicationUserId = booking.ApplicationUserId.ToString(),
                // Map any other properties you need to edit
            };

            BookingId = id;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                // Call the service update method
                await _bookingService.Update(Booking, id);
                return RedirectToPage("/booking/index", new { id = id }); // Redirect to the details page or any other page
            }
            catch (Exception ex)
            {
                // Handle any error that occurs during the update
                ModelState.AddModelError(string.Empty, $"Error updating service: {ex.Message}");
                return Page();
            }       
        }
    }
}
