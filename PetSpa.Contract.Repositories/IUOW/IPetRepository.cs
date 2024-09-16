﻿using PetSpa.Contract.Repositories.Entity;

namespace PetSpa.Contract.Repositories
{
    public interface IPetRepository
    {
        Task<IEnumerable<Pets>> GetAllPetsAsync();
        Task<Pets?> GetPetByIdAsync(Guid id);
        Task AddPetAsync(Pets pet);
        Task UpdatePetAsync(Pets pet);
        Task DeletePetAsync(Guid id);
    }
}