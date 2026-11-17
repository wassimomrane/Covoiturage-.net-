using Covoiturage.Application.DTOs.Trajets;
using Covoiturage.Application.Interfaces;
using Covoiturage.Domain.Entities;
using MediatR;

namespace Covoiturage.Application.Features.Trajets.Commands.CreateTrajet;

public record CreateTrajetCommand(
    string ConducteurId,
    int VilleDepartId,
    int VilleArriveeId,
    DateTime DateDepart,
    int VehiculeId,
    int PlacesDisponibles,
    decimal PrixParPlace
) : IRequest<TrajetResponseDto>;

public class CreateTrajetCommandHandler : IRequestHandler<CreateTrajetCommand, TrajetResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateTrajetCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TrajetResponseDto> Handle(CreateTrajetCommand request, CancellationToken cancellationToken)
    {
        var vehicule = await _unitOfWork.Vehicules.GetByIdAsync(request.VehiculeId)
            ?? throw new KeyNotFoundException("Véhicule introuvable.");

        if (vehicule.ConducteurId != request.ConducteurId)
            throw new UnauthorizedAccessException("Ce véhicule ne vous appartient pas.");

        if (request.PlacesDisponibles > vehicule.NombrePlaces)
            throw new InvalidOperationException($"Le nombre de places dépasse la capacité du véhicule ({vehicule.NombrePlaces}).");

        if (request.VilleDepartId == request.VilleArriveeId)
            throw new InvalidOperationException("La ville de départ et d'arrivée doivent être différentes.");

        var trajet = new Trajet
        {
            ConducteurId = request.ConducteurId,
            VilleDepartId = request.VilleDepartId,
            VilleArriveeId = request.VilleArriveeId,
            DateDepart = request.DateDepart,
            VehiculeId = request.VehiculeId,
            PlacesDisponibles = request.PlacesDisponibles,
            PrixParPlace = request.PrixParPlace
        };

        await _unitOfWork.Trajets.AddAsync(trajet);
        await _unitOfWork.CommitAsync();

        var depart = await _unitOfWork.Villes.GetByIdAsync(trajet.VilleDepartId);
        var arrivee = await _unitOfWork.Villes.GetByIdAsync(trajet.VilleArriveeId);

        return new TrajetResponseDto
        {
            Id = trajet.Id,
            VilleDepartNom = depart?.Nom ?? "",
            VilleArriveeNom = arrivee?.Nom ?? "",
            DateDepart = trajet.DateDepart,
            PlacesDisponibles = trajet.PlacesDisponibles,
            PrixParPlace = trajet.PrixParPlace,
            VehiculeMarque = vehicule.Marque,
            VehiculeModele = vehicule.Modele
        };
    }
}