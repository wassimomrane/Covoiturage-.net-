using Covoiturage.Application.Interfaces;
using Covoiturage.Domain.Entities;
using Covoiturage.Domain.Enums;
using MediatR;

namespace Covoiturage.Application.Features.Reservations.Commands.CreateReservation;

public record CreateReservationCommand(string PassagerId, int TrajetId, int NombrePlaces) : IRequest<int>;

public class CreateReservationCommandHandler : IRequestHandler<CreateReservationCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateReservationCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
    {
        var trajet = await _unitOfWork.Trajets.GetByIdAsync(request.TrajetId)
            ?? throw new KeyNotFoundException("Trajet introuvable.");

        if (trajet.ConducteurId == request.PassagerId)
            throw new InvalidOperationException("Vous ne pouvez pas réserver votre propre trajet.");

        if (request.NombrePlaces <= 0 || request.NombrePlaces > trajet.PlacesDisponibles)
            throw new InvalidOperationException("Nombre de places demandé invalide ou insuffisant.");

        var reservation = new Reservation
        {
            TrajetId = request.TrajetId,
            PassagerId = request.PassagerId,
            NombrePlaces = request.NombrePlaces,
            PrixTotal = request.NombrePlaces * trajet.PrixParPlace,
            Statut = StatutReservation.EnAttente,
            DateReservation = DateTime.UtcNow
        };

        await _unitOfWork.Reservations.AddAsync(reservation);
        await _unitOfWork.CommitAsync();

        return reservation.Id;
    }
}