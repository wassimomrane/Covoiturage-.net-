using Covoiturage.Application.Interfaces.Repositories;

namespace Covoiturage.Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IVilleRepository Villes { get; }
    IVehiculeRepository Vehicules { get; }
    ITrajetRepository Trajets { get; }
    IReservationRepository Reservations { get; }
    Task<int> CommitAsync();
}