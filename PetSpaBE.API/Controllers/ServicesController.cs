
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.Core.Store;
using PetSpa.Core.Utils;
using PetSpa.ModelViews.ServiceModelViews;
using PetSpa.Services.Service;
using System.IdentityModel.Tokens.Jwt;
namespace PetSpaBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IServicesService _servicesService;

        public ServicesController(IServicesService servicesService)
        {
            _servicesService = servicesService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllServices(int pageNumber, int pageSize)
        {

            var item = await _servicesService.GetAll(pageNumber,pageSize);
            return Ok(new BaseResponseModel<BasePaginatedList<ServiceResposeModel>>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: item));

        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddService(ServiceCreateModel service)
        {
            await _servicesService.Add(service);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status201Created,
                code: ResponseCodeConstants.SUCCESS,
                data: "Add service successful"));
        }
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteService(string id)
        {
            await _servicesService.Delete(id);
            return Ok(new BaseResponseModel<ServiceResposeModel>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                message: "Delete services successful"
                ));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetServiceById(string id)
        {
            var service = await _servicesService.GetById(id);
            return Ok(new BaseResponseModel<ServiceResposeModel>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: service
                ));
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateService(string id,ServiceUpdateModel service)
        {
            await _servicesService.Update(id,service);
            return Ok(
                new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "update service successful")
                );
        }
    }
}