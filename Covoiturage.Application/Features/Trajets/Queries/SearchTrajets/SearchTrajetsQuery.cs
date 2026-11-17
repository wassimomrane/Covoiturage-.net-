using Covoiturage.Application.Common.Models;
using Covoiturage.Application.DTOs.Trajets;
using Covoiturage.Application.Interfaces;
using MediatR;

namespace Covoiturage.Application.Features.Trajets.Queries.SearchTrajets;

public record SearchTrajetsQuery(
    int? VilleDepartId,
    int? VilleArriveeId,
    DateTime? Date,
    int Page = 1,
    int PageSize = 6
) : IRequest<PagedResult<TrajetResponseDto>>;

public class SearchTrajetsQueryHandler : IRequestHandler<SearchTrajetsQuery, PagedResult<TrajetResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public SearchTrajetsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PagedResult<TrajetResponseDto>> Handle(SearchTrajetsQuery request, CancellationToken cancellationToken)
    {
        var paged = await _unitOfWork.Trajets.SearchTrajetsAsync(
            request.VilleDepartId, request.VilleArriveeId, request.Date, request.Page, request.PageSize);

        var items = paged.Items.Select(t => new TrajetResponseDto
        {
            Id = t.Id,
            VilleDepartNom = t.VilleDepart.Nom,
            VilleArriveeNom = t.VilleArrivee.Nom,
            DateDepart = t.DateDepart,
            PlacesDisponibles = t.PlacesDisponibles,
            PrixParPlace = t.PrixParPlace,
            ConducteurNomComplet = $"{t.Conducteur.Prenom} {t.Conducteur.Nom}",
            VehiculeMarque = t.Vehicule.Marque,
            VehiculeModele = t.Vehicule.Modele
        }).ToList();

        return new PagedResult<TrajetResponseDto>(items, paged.TotalCount, paged.Page, paged.PageSize);
    }
}