using Covoiturage.Application.DTOs.Vehicules;
using Covoiturage.Application.Interfaces;
using Covoiturage.Domain.Entities;
using MediatR;

namespace Covoiturage.Application.Features.Referentiels;

public record CreateVehiculeCommand(string ProprietaireId, string Marque, string Modele, string Immatriculation, int NombrePlaces) : IRequest<int>;
public record GetMesVehiculesQuery(string ProprietaireId) : IRequest<IEnumerable<VehiculeResponseDto>>;

public class VehiculesHandlers :
    IRequestHandler<CreateVehiculeCommand, int>,
    IRequestHandler<GetMesVehiculesQuery, IEnumerable<VehiculeResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public VehiculesHandlers(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<int> Handle(CreateVehiculeCommand request, CancellationToken cancellationToken)
    {
        var vehicule = new Vehicule
        {
            ConducteurId = request.ProprietaireId,
            Marque = request.Marque,
            Modele = request.Modele,
            Immatriculation = request.Immatriculation,
            NombrePlaces = request.NombrePlaces
        };

        await _unitOfWork.Vehicules.AddAsync(vehicule);
        await _unitOfWork.CommitAsync();
        return vehicule.Id;
    }

    public async Task<IEnumerable<VehiculeResponseDto>> Handle(GetMesVehiculesQuery request, CancellationToken cancellationToken)
    {
        var list = await _unitOfWork.Vehicules.GetByConducteurIdAsync(request.ProprietaireId);
        return list.Select(v => new VehiculeResponseDto
        {
            Id = v.Id,
            Marque = v.Marque,
            Modele = v.Modele,
            Immatriculation = v.Immatriculation,
            NombrePlaces = v.NombrePlaces
        });
    }
}