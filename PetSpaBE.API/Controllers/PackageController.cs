
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
<<<<<<< HEAD
using PetSpa.ModelViews.ModelViews;
=======
using PetSpa.ModelViews.PackageModelViews;
>>>>>>> 05e26ddaeb54be6bb24dbaadf87ff8883bb5426d


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
<<<<<<< HEAD
            var packages = await _packageService.GetAll();
        
            int totalPackage = packages.Count;

            // Thực hiện phân trang
            var paginatedPackages = packages
                .Skip((pageNumber - 1) * pageSize) 
                .Take(pageSize)                    
                .ToList();                        

            // Tạo đối tượng BasePaginatedList để trả về
            var paginatedList = new BasePaginatedList<PackageResponseModel>(paginatedPackages, totalPackage, pageNumber, pageSize);

            return Ok(paginatedList);
=======
           var packages  = await _packageService.GetAll(pageNumber,pageSize);
            return Ok(new BaseResponseModel<BasePaginatedList<GETPackageViewModel>>(
                statusCode: StatusCodes.Status200OK,
                code:ResponseCodeConstants.SUCCESS,
                data:packages));
>>>>>>> 05e26ddaeb54be6bb24dbaadf87ff8883bb5426d
        }
        [HttpPost]
        public async Task<IActionResult> AddPackage([FromBody]POSTPackageViewModel packageVM)
        {
            await _packageService.Add(packageVM);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Add package successful"));
        }
        [HttpDelete]
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
            return Ok(new BaseResponseModel<GETPackageViewModel>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: package));
        }
        [HttpPut]
        public async Task<IActionResult> UpdatePackage(Packages packages)
        {
            try
            {
                await _packageService.Update(packages);
                return Ok();
            }
            catch 
            {
                return NotFound("Package not found!");
            }
        }

    }
}
