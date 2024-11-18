using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.ModelViews.RankModelViews;

namespace PetSpaBE.Razor.Pages.Ranks
{
    public class IndexModel : PageModel
    {
        private readonly IRankService _rankSerivce;
        public BasePaginatedList<GetRankViewModel> lst_Rank { get; private set; }

        public IndexModel(IRankService rankSerivce) { 
            _rankSerivce = rankSerivce;
        }
        public async Task OnGet(int page  =1, int size = 6)
        {
            lst_Rank = await _rankSerivce.GetAll(page, size);
        }
    }
}
