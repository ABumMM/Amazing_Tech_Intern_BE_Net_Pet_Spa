using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Services.Interface;
using PetSpa.ModelViews.RankModelViews;
using PetSpa.ModelViews.ServiceModelViews;
using PetSpa.Services.Service;

namespace PetSpaBE.Razor.Pages.Ranks
{
    public class UpdateModel : PageModel
    {
        private readonly IRankService _rankSerivce;
		public GetRankViewModel rank { get; private set; }

		public UpdateModel(IRankService rankSerivce)
        {
            _rankSerivce = rankSerivce;
		}
		public async Task<IActionResult> OnGet(string id)
		{
			// Get the service by ID to pre-populate the form for editing
			rank = await _rankSerivce.GetByID(id); // Ensure you have a method to get the service by ID
			if (rank == null)
			{
				return NotFound();
			}
			return Page();
		}
		
    }
}
