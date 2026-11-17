using Covoiturage.Application.Interfaces.Repositories;
using Covoiturage.Domain.Entities;
using Covoiturage.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Covoiturage.Infrastructure.Repositories;

public class ReservationRepository : Repository<Reservation>, IReservationRepository
{
    public ReservationRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<Reservation>> GetReservationsByPassagerAsync(string passagerId) =>
        await _dbSet.Include(r => r.Trajet)
                        .ThenInclude(t => t.VilleDepart)
                    .Include(r => r.Trajet)
                        .ThenInclude(t => t.VilleArrivee)
                    .Include(r => r.Trajet)
                        .ThenInclude(t => t.Conducteur)
                    .Where(r => r.PassagerId == passagerId)
                    .OrderByDescending(r => r.DateReservation)
                    .AsNoTracking()
                    .ToListAsync();

    public async Task<IEnumerable<Reservation>> GetDemandesByConducteurAsync(string conducteurId) =>
        await _dbSet.Include(r => r.Passager)
                    .Include(r => r.Trajet)
                        .ThenInclude(t => t.VilleDepart)
                    .Include(r => r.Trajet)
                        .ThenInclude(t => t.VilleArrivee)
                    .Where(r => r.Trajet.ConducteurId == conducteurId)
                    .OrderByDescending(r => r.DateReservation)
                    .AsNoTracking()
                    .ToListAsync();

    public async Task<Reservation?> GetDetailsByIdAsync(int reservationId) =>
        await _dbSet.Include(r => r.Passager)
                    .Include(r => r.Trajet)
                    .FirstOrDefaultAsync(r => r.Id == reservationId);
}