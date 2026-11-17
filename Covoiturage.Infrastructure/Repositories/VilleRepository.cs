using Covoiturage.Application.Interfaces.Repositories;
using Covoiturage.Domain.Entities;
using Covoiturage.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Covoiturage.Infrastructure.Repositories;

public class VilleRepository : Repository<Ville>, IVilleRepository
{
    public VilleRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Ville?> GetByNomAsync(string nom) =>
        await _dbSet.FirstOrDefaultAsync(v => v.Nom.ToLower() == nom.ToLower());
}