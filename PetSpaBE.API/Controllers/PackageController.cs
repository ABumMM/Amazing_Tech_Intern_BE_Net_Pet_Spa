using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;

<<<<<<< HEAD

=======
>>>>>>> 39145a9053671ca5fa5ac234fa1a5ae4c7496cac
namespace PetSpaBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageController : ControllerBase
    {
        private readonly IPackageService _packageService;

<<<<<<< HEAD
        public PackageController(IPackageService packageService) 
        {
            _packageService=packageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPackages(int PageNumber = 1, int PageSize=3)
        {
            IList<Packages> packages = await _packageService.GetAll();

            int totalItems = packages.Count();

            var pagedItems = packages.Skip((PageNumber - 1) * PageSize).Take(PageSize).ToList();

            var paginatedList = new BasePaginatedList<Packages>(
                pagedItems, // Danh sách các phần tử cho trang hiện tại
                totalItems, // Tổng số phần tử
                PageNumber, // Số trang hiện tại
                PageSize // Kích thước trang
            );
=======
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
>>>>>>> 39145a9053671ca5fa5ac234fa1a5ae4c7496cac

            return Ok(paginatedList);
        }
        [HttpPost]
<<<<<<< HEAD
        public async Task<IActionResult> AddPackage(Packages packages)
        {
            await _packageService.Add(packages);
            return Ok();
        }
        [HttpDelete]
        public async Task<IActionResult> DeletePackage(string id)
        {
            try
            {
                await _packageService.Delete(id);
                return Ok();
            }
            catch
            {
                return NotFound("Package not found!");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPackageById(string id)
        {
            var packages = await _packageService.GetById(id)??null;
            if (packages is null)
                return NotFound("Package not found!");
            return Ok(packages);
        }
        [HttpPut]
        public async Task<IActionResult> UpdatePackage(Packages packages)
        {
            try
            {
                await _packageService.Update(packages);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound("Package not found!");
            }
        }
=======
        public async Task<IActionResult> AddPackage(Packages package)
        {
            await _packageService.Add(package);
            return Ok();
        }
>>>>>>> 39145a9053671ca5fa5ac234fa1a5ae4c7496cac
    }
}
