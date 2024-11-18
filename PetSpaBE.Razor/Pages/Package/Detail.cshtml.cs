using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetSpa.Contract.Services.Interface;

namespace PetSpaBE.Razor.Pages.Package
{
    public class DetailModel : PageModel
    {
        private readonly IPackageService _packageService;
        private readonly IReviewService _reviewService;

        public DetailModel(IPackageService packageService, IReviewService reviewService)
        {
            _packageService = packageService;
            _reviewService=reviewService;
        }
        public async Task<IActionResult> OnGetAsync(string Id)
        {
            if (string.IsNullOrWhiteSpace(Id))
            {
                return NotFound();
            }

           var Package = await _packageService.GetServicesByPackageId(Id);
            if (Package == null)
            {
                return NotFound();
            }

            return Page();
        }

    }
}
