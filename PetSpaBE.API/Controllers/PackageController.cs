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

            return Ok(paginatedList);
        }
        [HttpPost]
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
    }
}
