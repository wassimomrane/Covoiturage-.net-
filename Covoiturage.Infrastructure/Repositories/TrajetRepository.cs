using Covoiturage.Application.Common.Models;
using Covoiturage.Application.Interfaces.Repositories;
using Covoiturage.Domain.Entities;
using Covoiturage.Domain.Enums;
using Covoiturage.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Covoiturage.Infrastructure.Repositories;

public class TrajetRepository : Repository<Trajet>, ITrajetRepository
{
    public TrajetRepository(ApplicationDbContext context) : base(context) { }

    public async Task<PagedResult<Trajet>> SearchTrajetsAsync(int? villeDepartId, int? villeArriveeId, DateTime? date, int page, int pageSize)
    {
        // Seuil : 30 minutes après l'heure de départ
        var expirationSeuil = DateTime.UtcNow.AddMinutes(-30);

        var query = _dbSet.Include(t => t.VilleDepart)
                          .Include(t => t.VilleArrivee)
                          .Include(t => t.Vehicule)
                          .Include(t => t.Conducteur)
                          .Where(t => t.Statut == StatutTrajet.Planifie
                                   && t.PlacesDisponibles > 0
                                   && t.DateDepart >= expirationSeuil) 
                          .AsNoTracking();

        if (villeDepartId.HasValue)
            query = query.Where(t => t.VilleDepartId == villeDepartId.Value);

        if (villeArriveeId.HasValue)
            query = query.Where(t => t.VilleArriveeId == villeArriveeId.Value);

        if (date.HasValue)
            query = query.Where(t => t.DateDepart.Date == date.Value.Date);

        var total = await query.CountAsync();
        var items = await query.OrderBy(t => t.DateDepart)
                               .Skip((page - 1) * pageSize)
                               .Take(pageSize)
                               .ToListAsync();

        return new PagedResult<Trajet>(items, total, page, pageSize);
    }

    public async Task<IEnumerable<Trajet>> GetHistoriqueConducteurAsync(string conducteurId) =>
        await _dbSet.Include(t => t.VilleDepart)
                    .Include(t => t.VilleArrivee)
                    .Include(t => t.Vehicule)
                    .Where(t => t.ConducteurId == conducteurId)
                    .OrderByDescending(t => t.DateDepart)
                    .AsNoTracking()
                    .ToListAsync();

    public async Task<Trajet?> GetDetailsWithVehiculeAndVillesAsync(int trajetId) =>
        await _dbSet.Include(t => t.VilleDepart)
                    .Include(t => t.VilleArrivee)
                    .Include(t => t.Vehicule)
                    .Include(t => t.Conducteur)
                    .FirstOrDefaultAsync(t => t.Id == trajetId);
}