using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Services.Interface;
using PetSpa.ModelViews.PackageModelViews;
using PetSpa.ModelViews.PackageServiceModelViews;
using PetSpa.ModelViews.RankModelViews;
using PetSpa.ModelViews.ReviewModelViews;
using System.Drawing.Printing;

namespace PetSpaBE.Razor.Pages.Package
{
    public class DetailModel : PageModel
    {
        private readonly IPackageService _packageService;
        private readonly IReviewService _reviewService;
        public GETPackageModelView Package { get;  set; }
        public List<GETPackageServiceModelView> lstService { get; set; }

        public List<GETReviewModelViews> Reviews { get; set; }
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

            Package = await _packageService.GetById(Id);
            lstService = await _packageService.GetServicesByPackageId(Id);
            var reviews = await _reviewService.GetAllReviewsInPackage(Id, 1, 10);
            Reviews = reviews.Items.ToList();
            if (Package == null)
            {
                return NotFound();
            }

            return Page();
        }

    }
}
