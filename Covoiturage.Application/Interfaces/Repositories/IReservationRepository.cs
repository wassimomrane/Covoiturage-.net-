using Covoiturage.Domain.Entities;

namespace Covoiturage.Application.Interfaces.Repositories;

public interface IReservationRepository : IRepository<Reservation>
{
    Task<IEnumerable<Reservation>> GetReservationsByPassagerAsync(string passagerId);
    Task<IEnumerable<Reservation>> GetDemandesByConducteurAsync(string conducteurId);
    Task<Reservation?> GetDetailsByIdAsync(int reservationId);
}