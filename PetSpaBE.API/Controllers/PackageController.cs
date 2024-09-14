using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;

namespace PetSpaBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageController : ControllerBase
    {
        private readonly IPackageService _packageService;

        public PackageController(IPackageService packageService)
        {
            _packageService = packageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPackages(int pageNumber=1, int pageSize=2)
        {
            IList<Packages> packages = await _packageService.GetAll();
        
            int totalPackage = packages.Count;

            // Thực hiện phân trang
            var paginatedRoles = packages
                .Skip((pageNumber - 1) * pageSize) 
                .Take(pageSize)                    
                .ToList();                        

            // Tạo đối tượng BasePaginatedList để trả về
            var paginatedList = new BasePaginatedList<Packages>(paginatedRoles, totalPackage, pageNumber, pageSize);

            return Ok(paginatedList);
        }
        [HttpPost]
        public async Task<IActionResult> AddPackage(Packages package)
        {
            await _packageService.Add(package);
            return Ok();
        }
    }
}
