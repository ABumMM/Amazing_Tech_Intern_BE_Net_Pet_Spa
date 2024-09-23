using Microsoft.EntityFrameworkCore;
using PetSpa.Contract.Repositories;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Repositories.Context;
using System;
using System.Collections.Generic;
using System.Linq;
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

        // Thêm chức năng phân trang
        public async Task<IEnumerable<Pets>> GetAllPetsAsync(int pageNumber, int pageSize)
        {
            // Bỏ qua các mục trước đó và lấy số lượng mục cần thiết
            return await _context.Pets
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
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
