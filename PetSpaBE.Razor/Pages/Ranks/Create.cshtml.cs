using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Services.Interface;
using PetSpa.ModelViews.RankModelViews;

namespace PetSpaBE.Razor.Pages.Ranks
{
    public class CreateModel : PageModel
    {
        private readonly IRankService _rankService;

        [BindProperty]
        public PostRankViewModel Rank { get; set; }

        public CreateModel(IRankService rankService)
        {
            _rankService = rankService;
        }

        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _rankService.Add(Rank);
            return RedirectToPage("./AdminRanks");
        }
    }
}
