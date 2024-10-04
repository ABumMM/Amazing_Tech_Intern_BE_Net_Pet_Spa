
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
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
        [HttpGet("conditions")]
        public async Task<IActionResult> GetPackageByConditions(DateTimeOffset? DateStart, DateTimeOffset? DateEnd)
        {
            var package = await _packageService.GetPackageByConditions(DateStart,DateEnd);
            return Ok(new BaseResponseModel<List<GETPackageModelView>>(
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
        [HttpDelete("ServiceInPackageID")]
        public async Task<IActionResult> DeleteServiceInPackage(string serviceINPackageID)
        {
            await _packageService.DeleteServiceInPakcage(serviceINPackageID);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Delete service successful"));
        }
    }
}
