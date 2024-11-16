using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetSpa.Contract.Services.Interface;
using PetSpa.ModelViews.ServiceModelViews;

namespace PetSpaBE.Razor.Pages.Services
{
    public class UpdateModel : PageModel
    {
		private readonly IServicesService _serviceService;

		[BindProperty]
		public ServiceUpdateModel Service { get; set; }

		public string ServiceId { get; set; }

		public UpdateModel(IServicesService serviceService)
		{
			_serviceService = serviceService;
		}

		public async Task<IActionResult> OnGetAsync(string id)
		{
			// Get the service by ID to pre-populate the form for editing
			var service = await _serviceService.GetById(id); // Ensure you have a method to get the service by ID
			if (service == null)
			{
				return NotFound();
			}

			Service = new ServiceUpdateModel
			{
				Name = service.Name,
				Description = service.Description,
				// Map any other properties you need to edit
			};

			ServiceId = id;
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
				await _serviceService.Update(id, Service);
				return RedirectToPage("/Services/Details", new { id = id }); // Redirect to the details page or any other page
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

