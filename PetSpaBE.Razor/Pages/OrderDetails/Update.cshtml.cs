using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetSpa.Contract.Services.Interface;
using PetSpa.ModelViews.OrderDetailModelViews;

namespace PetSpaBE.Razor.Pages.OrderDetails
{
    public class UpdateOrDetailModel : PageModel
    {
        private readonly IOrderDetailServices _orderDetailServices;

        [BindProperty]
        public PUTOrderDetailModelView OrderDetail { get; set; }

        public string OrderDetailId { get; set; }

        public UpdateOrDetailModel(IOrderDetailServices orderDetailServices)
        {
            _orderDetailServices = orderDetailServices;
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            // Get the service by ID to pre-populate the form for editing
            var orDetail = await _orderDetailServices.GetById(id); // Ensure you have a method to get the service by ID
            if (orDetail == null)
            {
                return NotFound();
            }

            OrderDetail = new PUTOrderDetailModelView
            {
               Id = orDetail.Id,
               OrderID = orDetail.OrderID,
               Quantity = orDetail.Quantity,
               Status = orDetail.Status
            };

            OrderDetailId = id;
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
                await _orderDetailServices.Update(OrderDetail);
                return RedirectToPage("/OrderDetails/AdminOrderDetail", new { id = id }); // Redirect to the details page or any other page
            }
            catch (Exception ex)
            {
                // Handle any error that occurs during the update
                ModelState.AddModelError(string.Empty, $"Error updating Order Detail: {ex.Message}");
                return Page();
            }
        }
    }
}
