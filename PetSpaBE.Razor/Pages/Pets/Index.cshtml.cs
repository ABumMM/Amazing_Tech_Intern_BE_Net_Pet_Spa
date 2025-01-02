using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.ModelViews.PetsModelViews;

namespace PetSpaBE.Razor.Pages.Pets
{
    public class IndexModel : PageModel
    {
        private readonly IPetService _petService;

        public IndexModel(IPetService petService)
        {
            _petService = petService;
        }

        public BasePaginatedList<GETPetsModelView> lst_Pets { get; private set; }

        public async Task OnGet(int page = 1, int size = 6)
        {
            lst_Pets = await _petService.GetAll(pageNumber: page, pageSize: size);
        }
    }
}
