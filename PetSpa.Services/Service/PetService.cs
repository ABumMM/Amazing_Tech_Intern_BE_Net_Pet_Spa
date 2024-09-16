using PetSpa.Contract.Repositories;
using PetSpa.Contract.Services;
using PetSpa.Contract.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PetSpa.Repositories;



namespace PetSpa.Services
{
    public class PetService : IPetService
    {
        private readonly IPetRepository _petRepository;

        public PetService(IPetRepository petRepository)
        {
            _petRepository = petRepository;
        }

        public async Task<IEnumerable<Pets>> GetAllPetsAsync()
        {
            return await _petRepository.GetAllPetsAsync();
        }

        public async Task<Pets?> GetPetByIdAsync(Guid id)
        {
            return await _petRepository.GetPetByIdAsync(id);
        }

        public async Task AddPetAsync(Pets pet)
        {
            await _petRepository.AddPetAsync(pet);
        }

        public async Task UpdatePetAsync(Pets pet)
        {
            await _petRepository.UpdatePetAsync(pet);
        }

        public async Task DeletePetAsync(Guid id)
        {
            await _petRepository.DeletePetAsync(id);
        }
    }
}
