using PetSpa.Contract.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.Contract.Services.Interface
{
    public interface IRoleService
    {
        Task<ApplicationRole> GetByIdAsync(Guid id);
        Task<IEnumerable<ApplicationRole>> GetAllAsync();
        Task AddAsync(ApplicationRole role);
        Task UpdateAsync(ApplicationRole role);
        Task DeleteAsync(Guid id);
    }
}
