using Core.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.ModelViews.ModelViews;


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
        public async Task<IActionResult> GetAllPackages(int pageNumber=1, int pageSize=2)
        {
           var packages  = await _packageService.GetAll(pageNumber,pageSize);
            return Ok(new BaseResponseModel<BasePaginatedList<PackageResponseModel>>(
                statusCode: StatusCodes.Status200OK,
                code:ResponseCodeConstants.SUCCESS,
                data:packages));
        }
        [HttpPost]
        public async Task<IActionResult> AddPackage(Packages packages)
        {
            await _packageService.Add(packages);
            return Ok();
        }
        [HttpDelete]
        public async Task<IActionResult> DeletePackage(Guid id)
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
        public async Task<IActionResult> GetPackageById(Guid id)
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
