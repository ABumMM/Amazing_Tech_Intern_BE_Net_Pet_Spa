using Microsoft.EntityFrameworkCore;
using PetSpa.Contract.Repositories;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Repositories.Context;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PetSpa.Repositories
{
    public class PetRepository : IPetRepository
    {
        private readonly DatabaseContext _context;

        public PetRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Pets>> GetAllPetsAsync()
        {
            return await _context.Pets.ToListAsync();
        }

        public async Task<Pets?> GetPetByIdAsync(Guid id)
        {
            return await _context.Pets.FindAsync(id);
        }

        public async Task AddPetAsync(Pets pet)
        {
            await _context.Pets.AddAsync(pet);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePetAsync(Pets pet)
        {
            _context.Pets.Update(pet);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePetAsync(Guid id)
        {
            var pet = await _context.Pets.FindAsync(id);
            if (pet != null)
            {
                _context.Pets.Remove(pet);
                await _context.SaveChangesAsync();
            }
        }
    }
}
