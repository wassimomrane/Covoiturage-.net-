using Covoiturage.Application.Common.Models;
using Covoiturage.Domain.Entities;

namespace Covoiturage.Application.Interfaces.Repositories;

public interface ITrajetRepository : IRepository<Trajet>
{
    Task<PagedResult<Trajet>> SearchTrajetsAsync(int? villeDepartId, int? villeArriveeId, DateTime? date, int page, int pageSize);
    Task<IEnumerable<Trajet>> GetHistoriqueConducteurAsync(string conducteurId);
    Task<Trajet?> GetDetailsWithVehiculeAndVillesAsync(int trajetId);
}