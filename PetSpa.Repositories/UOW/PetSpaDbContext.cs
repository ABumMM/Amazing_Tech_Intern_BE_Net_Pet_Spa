
using Microsoft.EntityFrameworkCore;
using PetSpa.Contract.Repositories.Entity;

namespace PetSpa.Repositories
{
    public class PetSpaDbContext
    {
        public DbSet<Pets> Pets { get; set; }

        internal async Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}