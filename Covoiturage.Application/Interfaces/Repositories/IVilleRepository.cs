using Covoiturage.Domain.Entities;

namespace Covoiturage.Application.Interfaces.Repositories;

public interface IVilleRepository : IRepository<Ville>
{
    Task<Ville?> GetByNomAsync(string nom);
}