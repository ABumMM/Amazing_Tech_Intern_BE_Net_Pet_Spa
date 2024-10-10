using Microsoft.AspNetCore.Mvc;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.Core.Infrastructure;
using PetSpa.ModelViews.PackageModelViews;
using PetSpa.ModelViews.PackageServiceModelViews;
using Swashbuckle.AspNetCore.Annotations;
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
        [HttpGet("all")]
        [SwaggerOperation(
            Summary = "Authorization: Customer",
            Description = "View all packages"
            )]
        public async Task<IActionResult> GetAllPackages(int pageNumber, int pageSize)
        {
            var packages = await _packageService.GetAll(pageNumber, pageSize);
            return Ok(new BaseResponseModel<BasePaginatedList<GETPackageModelView>>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: packages));
        }
        [HttpPost("add-package")]
        [SwaggerOperation(
            Summary = "Authorization: Admin",
            Description = "Add package by admin"
            )]
        public async Task<IActionResult> AddPackage([FromBody] POSTPackageModelView packageVM)
        {
            await _packageService.Add(packageVM);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Add package successful"));
        }
        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Authorization: Admin",
            Description = "Delete a package by admin"
            )]
        public async Task<IActionResult> DeletePackage(string id)
        {
            await _packageService.Delete(id);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Delete package successful"));
        }

        [HttpGet("by-id")]
        [SwaggerOperation(
            Summary = "Authorization: Customer",
            Description = "View package By Package ID"
            )]
        public async Task<IActionResult> GetPackageById(string id)
        {
            var package = await _packageService.GetById(id);
            return Ok(new BaseResponseModel<GETPackageModelView>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: package));
        }
        [HttpPost("add-service")]
        [SwaggerOperation(
            Summary = "Authorization: Admin",
            Description = "Add service to Package (packageID, serviceID)"
            )]
        public async Task<IActionResult> AddServiceToPackage(string packageID, string serviceID)
        {
            await _packageService.AddServiceInPackage(packageID, serviceID);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Add service to package successful"));
        }

        [HttpGet("all-service")]
        [SwaggerOperation(
            Summary = "Authorization: Anyone",
            Description = "Get all services in package By PackageID"
            )]
        public async Task<IActionResult> GetServicesByPackageId(string packageId)
        {
            var packages = await _packageService.GetServicesByPackageId(packageId);
            return Ok(new BaseResponseModel<List<GETPackageServiceModelView>>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: packages));
        }

        [HttpGet("by-conditions")]
        [SwaggerOperation(
            Summary = "Authorization: Anyone",
            Description = "Get all services in package By DataTime (DateStart,DateEnd)"
            )]
        public async Task<IActionResult> GetPackageByConditions(DateTimeOffset? DateStart, DateTimeOffset? DateEnd)
        {
            var package = await _packageService.GetPackageByConditions(DateStart, DateEnd);
            return Ok(new BaseResponseModel<List<GETPackageModelView>>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: package));
        }
        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Authorization: Admin",
            Description = "Update package"
            )]
        public async Task<IActionResult> UpdatePackage([FromBody] PUTPackageModelView packageMV)
        {
            await _packageService.Update(packageMV);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Update package successful"));
        }
        [HttpDelete("serviceINPackageID")]
        [SwaggerOperation(
            Summary = "Authorization: Admin",
            Description = "Delete service In Package By serviceINPackageID"
            )]
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
