using Covoiturage.Domain.Entities;

namespace Covoiturage.Application.Interfaces.Repositories;

public interface IVehiculeRepository : IRepository<Vehicule>
{
    Task<IEnumerable<Vehicule>> GetByConducteurIdAsync(string conducteurId);
}