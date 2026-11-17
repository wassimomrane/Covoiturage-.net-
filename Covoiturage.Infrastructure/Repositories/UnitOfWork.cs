using Covoiturage.Application.Interfaces;
using Covoiturage.Application.Interfaces.Repositories;
using Covoiturage.Infrastructure.Data;

namespace Covoiturage.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public IVilleRepository Villes { get; }
    public IVehiculeRepository Vehicules { get; }
    public ITrajetRepository Trajets { get; }
    public IReservationRepository Reservations { get; }

    public UnitOfWork(
        ApplicationDbContext context,
        IVilleRepository villes,
        IVehiculeRepository vehicules,
        ITrajetRepository trajets,
        IReservationRepository reservations)
    {
        _context = context;
        Villes = villes;
        Vehicules = vehicules;
        Trajets = trajets;
        Reservations = reservations;
    }

    public async Task<int> CommitAsync() => await _context.SaveChangesAsync();

    public void Dispose() => _context.Dispose();
}