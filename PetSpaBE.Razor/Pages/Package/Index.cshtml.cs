using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.ModelViews.PackageModelViews;
using PetSpa.ModelViews.ServiceModelViews;

namespace PetSpaBE.Razor.Pages.Package
{
    public class IndexModel : PageModel
    {
		private readonly IPackageService _packageService;
		public IndexModel(IPackageService packageService)
		{
			_packageService = packageService;
		}

		public BasePaginatedList<GETPackageModelView> lst_Packages { get;set; }

		public async Task OnGet(int page, int size )
		{
			lst_Packages = await _packageService.GetAll(1,10);
		}
	}
}
