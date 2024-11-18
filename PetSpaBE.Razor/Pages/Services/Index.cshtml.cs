using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.ModelViews.RankModelViews;
using PetSpa.ModelViews.ServiceModelViews;

namespace PetSpaBE.Razor.Pages.Services
{
    public class IndexModel : PageModel
    {
        private readonly IServicesService _serviceService;
        public IndexModel(IServicesService ServicesService)
        {
            _serviceService = ServicesService;
        }

		public BasePaginatedList<ServiceResposeModel> lst_Services { get; private set; }

		// Thuộc tính để lưu danh sách rank

		public async Task OnGet(int page = 1 , int size = 6 ,string type = "Customer")
        {
            lst_Services=  await _serviceService.GetAll(pageNumber:1,pageSize:6);
            
        }
    }
}
