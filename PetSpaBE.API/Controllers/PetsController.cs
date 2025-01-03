﻿using Microsoft.AspNetCore.Mvc;
using PetSpa.Core.Base;
using PetSpa.ModelViews.PetsModelViews;
using PetSpa.Contract.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;

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
        public async Task<IActionResult> AddPet([FromBody] POSTPetsModelView pet)
        {
            await _petService.Add(pet);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Add pet successful"));
        }
    

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePets(string id) 
        {
            await _petService.Delete(id);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Delete pet successful"));
        }
        [HttpGet("{id}")]

        public async Task<IActionResult> GetPetsById(string id)
        {
            var pet = await _petService.GetById(id);
            return Ok(new BaseResponseModel<GETPetsModelView>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: pet));
        }
        [HttpPost("add-service")]
        [SwaggerOperation(
            Summary = "Authorization: Admin",
            Description = "Add service to Package (packageID, serviceID)"
            )]
        [Authorize(Roles = "Admin")]
        
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
