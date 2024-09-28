using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.ModelViews.PackageModelViews;
using PetSpa.ModelViews.PackageServiceModelViews;
using PetSpa.Services.Service;

namespace PetSpaBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageServiceController : ControllerBase
    {
        private readonly IPackageService_Service _packageService_Service;

        public PackageServiceController(IPackageService_Service packageService_Service)
        {
            _packageService_Service = packageService_Service;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllPackages(int pageNumber = 1, int pageSize = 2)
        {
            var data = await _packageService_Service.GetAll(pageNumber, pageSize);
            return Ok(new BaseResponseModel<BasePaginatedList<GETPackageServiceModelView>>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: data));
        }
        [HttpPost]
        public async Task<IActionResult> AddServiceToPackage(string packageID,string serviceID)
        {
            await _packageService_Service.Add(packageID,serviceID);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Add package successful"));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPackageServuceById(string id)
        {
            var package = await _packageService_Service.GetById(id);
            return Ok(new BaseResponseModel<GETPackageServiceModelView>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: package));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServiceInPackge(string id)
        {
            await _packageService_Service.Delete(id);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Delete service package successful"));
        }
    }
}
