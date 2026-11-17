using Covoiturage.Application.DTOs.Reservations;
using Covoiturage.Application.Interfaces;
using MediatR;

namespace Covoiturage.Application.Features.Reservations.Queries.GetReservations;

public record GetMesReservationsQuery(string PassagerId) : IRequest<IEnumerable<ReservationResponseDto>>;
public record GetDemandesConducteurQuery(string ConducteurId) : IRequest<IEnumerable<ReservationResponseDto>>;

public class GetReservationsHandlers :
    IRequestHandler<GetMesReservationsQuery, IEnumerable<ReservationResponseDto>>,
    IRequestHandler<GetDemandesConducteurQuery, IEnumerable<ReservationResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetReservationsHandlers(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<ReservationResponseDto>> Handle(GetMesReservationsQuery request, CancellationToken cancellationToken)
    {
        var list = await _unitOfWork.Reservations.GetReservationsByPassagerAsync(request.PassagerId);
        return list.Select(r => new ReservationResponseDto
        {
            Id = r.Id,
            TrajetId = r.TrajetId,
            VilleDepartNom = r.Trajet.VilleDepart.Nom,
            VilleArriveeNom = r.Trajet.VilleArrivee.Nom,
            DateDepart = r.Trajet.DateDepart,
            NombrePlaces = r.NombrePlaces,
            PrixTotal = r.PrixTotal,
            Statut = r.Statut,
            ConducteurNomComplet = $"{r.Trajet.Conducteur.Prenom} {r.Trajet.Conducteur.Nom}",
            ConducteurPhoneNumber = r.Trajet.Conducteur.PhoneNumber ?? ""
        });
    }

    public async Task<IEnumerable<ReservationResponseDto>> Handle(GetDemandesConducteurQuery request, CancellationToken cancellationToken)
    {
        var list = await _unitOfWork.Reservations.GetDemandesByConducteurAsync(request.ConducteurId);
        return list.Select(r => new ReservationResponseDto
        {
            Id = r.Id,
            TrajetId = r.TrajetId,
            VilleDepartNom = r.Trajet.VilleDepart.Nom,
            VilleArriveeNom = r.Trajet.VilleArrivee.Nom,
            DateDepart = r.Trajet.DateDepart,
            NombrePlaces = r.NombrePlaces,
            PrixTotal = r.PrixTotal,
            Statut = r.Statut,
            PassagerNomComplet = $"{r.Passager.Prenom} {r.Passager.Nom}",
            PassagerPhoneNumber = r.Passager.PhoneNumber ?? ""
        });
    }
}