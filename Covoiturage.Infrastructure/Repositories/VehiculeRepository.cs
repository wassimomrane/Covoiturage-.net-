using Covoiturage.Application.Interfaces.Repositories;
using Covoiturage.Domain.Entities;
using Covoiturage.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Covoiturage.Infrastructure.Repositories;

public class VehiculeRepository : Repository<Vehicule>, IVehiculeRepository
{
    public VehiculeRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<Vehicule>> GetByConducteurIdAsync(string conducteurId) =>
        await _dbSet.Where(v => v.ConducteurId == conducteurId).AsNoTracking().ToListAsync();
}