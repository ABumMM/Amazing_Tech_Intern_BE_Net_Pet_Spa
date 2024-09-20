using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.ModelViews.ServiceModelViews;
using PetSpa.Services.Service;
namespace PetSpaBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IServicesService _servicesService;

        public ServicesController(IServicesService servicesService) {
            _servicesService = servicesService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllServices(int pageNumber=1,int pageSize=10) {

            IList<ServicesEntity> services = await _servicesService.GetAll();

            // Map to ServiceResponseModel
            IList<ServiceResposeModel> serviceResponseModels = services.Select(s => new ServiceResposeModel
            {
                Name = s.Name,
                Description = s.Description,
                PackageId = s.PackageId,
                
            }).ToList();

            // Total number of services
            int totalServices = serviceResponseModels.Count;

            // Paginate the results
            var paginatedServices = serviceResponseModels
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Create the paginated list
            var paginatedList = new BasePaginatedList<ServiceResposeModel>(paginatedServices, totalServices, pageNumber, pageSize);

            // Return the paginated result
            return Ok(paginatedList);
        }

        [HttpPost]
        public async Task<IActionResult> AddService(ServiceCreateModel service)
        {
            ServicesEntity servicesEntity = new ServicesEntity{
                Id = Guid.NewGuid().ToString("N"),
                Name = service.Name,
                Description = service.Description,
                PackageId = service.PackageId,
            };
            await _servicesService.Add(servicesEntity);
            return Ok();
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteService(string id)
        {
            try
            {
                await _servicesService.Delete(id);
                return Ok("Delete Sucessfull");
            }
            catch
            {
                return NotFound("Package not found!");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetServiceById(string id)
        {
            var services = await _servicesService.GetById(id) ?? null;
            if (services is null)
                return NotFound("Package not found!");
            return Ok(services);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateService(ServiceUpdateModel service)
        {
            try
            {
                ServicesEntity servicesEntity = new ServicesEntity
                {
                    Id = service.Id,
                    Name = service.Name,
                    Description = service.Description,
                    PackageId = service.PackageId,
                    Price = service.Price,
                    LastUpdatedTime = DateTime.Now
                };
                await _servicesService.Update(servicesEntity);
                return Ok(servicesEntity);
            }
            catch
            {
                return NotFound("Package not found!");
            }
        }
    }
}
