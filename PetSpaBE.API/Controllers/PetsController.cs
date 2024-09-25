using Microsoft.AspNetCore.Mvc;
using PetSpa.Contract.Services;
using PetSpa.Contract.Repositories.Entity;
using System;
using System.Threading.Tasks;
using PetSpa.Core.Base;
using PetSpa.ModelViews.RoleModelViews;
using PetSpa.ModelViews.PetsModelViews;

namespace PetSpaBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetsController : ControllerBase
    {
        private readonly IPetService _petService;

        public PetsController(IPetService petService)
        {
            _petService = petService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPets(int pageNumber = 1, int pageSize = 2)
        {
            var pets = await _petService.GetAll(pageNumber, pageSize);
            return Ok(new BaseResponseModel<BasePaginatedList<GETPetsModelView>>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: pets));
        }

        [HttpPost]
        public async Task<IActionResult> AddPets([FromBody] POSTPetsModelView pet)
        {
            await _petService.Add(pet);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Add pets successful"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePets(string Id)
        {
            await _petService.Delete(Id);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Delete pet successful"));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPetsById(string Id)
        {
            var pet = await _petService.GetById(Id);
            return Ok(new BaseResponseModel<GETPetsModelView>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: pet));
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePets([FromBody] PUTPetsModelView pet)
        {
            await _petService.Update(pet);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Update pet successful"));
        }
    }
}
