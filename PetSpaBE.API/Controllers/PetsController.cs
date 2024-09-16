using Microsoft.AspNetCore.Mvc;
using PetSpa.Contract.Services;
using PetSpa.Contract.Repositories.Entity;
using System;
using System.Threading.Tasks;

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
        public async Task<IActionResult> GetAllPets()
        {
            var pets = await _petService.GetAllPetsAsync();
            return Ok(pets);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPetById(Guid id)
        {
            var pet = await _petService.GetPetByIdAsync(id);
            if (pet == null)
                return NotFound();

            return Ok(pet);
        }

        [HttpPost]
        public async Task<IActionResult> AddPet([FromBody] Pets pet)
        {
            await _petService.AddPetAsync(pet);
            return CreatedAtAction(nameof(GetPetById), new { id = pet.Id }, pet);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePet(Guid id, [FromBody] Pets pet)
        {
            if (id != pet.Id)
                return BadRequest();

            await _petService.UpdatePetAsync(pet);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePet(Guid id)
        {
            await _petService.DeletePetAsync(id);
            return NoContent();
        }
    }
}
