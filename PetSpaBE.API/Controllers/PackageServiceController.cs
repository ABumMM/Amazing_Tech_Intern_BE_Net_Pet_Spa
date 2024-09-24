using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.ModelViews.PackageModelViews;
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
        [HttpPost]
        public async Task<IActionResult> AddPackageService(string packageID,string serviceID)
        {
            await _packageService_Service.Add(packageID,serviceID);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Add package successful"));
        }
    }
}
