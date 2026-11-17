using Covoiturage.Application.Interfaces;
using Covoiturage.Domain.Enums;
using MediatR;

namespace Covoiturage.Application.Features.Reservations.Commands.TraiterReservation;

public enum ActionReservation
{
    Confirmer,
    Refuser,
    AnnulerPassager,
    AccepterAnnulationConducteur
}

public record TraiterReservationCommand(int ReservationId, string UserId, ActionReservation Action) : IRequest<string>;

public class TraiterReservationCommandHandler : IRequestHandler<TraiterReservationCommand, string>
{
    private readonly IUnitOfWork _unitOfWork;

    public TraiterReservationCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<string> Handle(TraiterReservationCommand request, CancellationToken cancellationToken)
    {
        var reservation = await _unitOfWork.Reservations.GetDetailsByIdAsync(request.ReservationId)
            ?? throw new KeyNotFoundException("Réservation introuvable.");

        switch (request.Action)
        {
            case ActionReservation.Confirmer:
                if (reservation.Trajet.ConducteurId != request.UserId)
                    throw new UnauthorizedAccessException("Action non autorisée.");
                if (reservation.Statut != StatutReservation.EnAttente)
                    throw new InvalidOperationException("Cette réservation n'est pas en attente.");
                if (reservation.Trajet.PlacesDisponibles < reservation.NombrePlaces)
                    throw new InvalidOperationException("Plus assez de places disponibles sur ce trajet.");

                reservation.Statut = StatutReservation.Confirmee;
                reservation.Trajet.PlacesDisponibles -= reservation.NombrePlaces;
                await _unitOfWork.CommitAsync();
                return "Réservation confirmée et places mises à jour.";

            case ActionReservation.Refuser:
                if (reservation.Trajet.ConducteurId != request.UserId)
                    throw new UnauthorizedAccessException("Action non autorisée.");
                reservation.Statut = StatutReservation.Refusee;
                await _unitOfWork.CommitAsync();
                return "Réservation refusée.";

            case ActionReservation.AnnulerPassager:
                if (reservation.PassagerId != request.UserId)
                    throw new UnauthorizedAccessException("Action non autorisée.");
                if (reservation.Statut == StatutReservation.EnAttente)
                {
                    reservation.Statut = StatutReservation.Annulee;
                    await _unitOfWork.CommitAsync();
                    return "Réservation annulée.";
                }
                if (reservation.Statut == StatutReservation.Confirmee)
                {
                    reservation.Statut = StatutReservation.DemandeAnnulation;
                    await _unitOfWork.CommitAsync();
                    return "Demande d'annulation envoyée au conducteur.";
                }
                throw new InvalidOperationException("Impossible d'annuler cette réservation.");

            case ActionReservation.AccepterAnnulationConducteur:
                if (reservation.Trajet.ConducteurId != request.UserId)
                    throw new UnauthorizedAccessException("Action non autorisée.");
                if (reservation.Statut != StatutReservation.DemandeAnnulation)
                    throw new InvalidOperationException("Aucune demande d'annulation en cours.");

                reservation.Statut = StatutReservation.Annulee;
                reservation.Trajet.PlacesDisponibles += reservation.NombrePlaces;
                await _unitOfWork.CommitAsync();
                return "Annulation acceptée et places libérées.";

            default:
                throw new InvalidOperationException("Action inconnue.");
        }
    }
}