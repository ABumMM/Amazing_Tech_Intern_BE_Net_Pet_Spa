
using Microsoft.AspNetCore.Mvc;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.ModelViews.PackageModelViews;

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
        public async Task<IActionResult> GetAllPackages(int pageNumber = 1, int pageSize = 10)
        {
            var packages = await _packageService.GetAll(pageNumber, pageSize);
            return Ok(new BaseResponseModel<BasePaginatedList<GETPackageModelView>>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: packages));
        }
        [HttpPost]
        public async Task<IActionResult> AddPackage([FromBody] POSTPackageModelView packageVM)
        {
            await _packageService.Add(packageVM);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Add package successful"));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePackage(string id)
        {
            await _packageService.Delete(id);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Delete package successful"));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPackageById(string id)
        {
            var package = await _packageService.GetById(id);
            return Ok(new BaseResponseModel<GETPackageModelView>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: package));
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePackage([FromBody] PUTPackageModelView packageMV)
        {
            await _packageService.Update(packageMV);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Update package successful"));
        }

    }
}
