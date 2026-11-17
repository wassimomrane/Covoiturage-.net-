using Covoiturage.Application.DTOs.Villes;
using Covoiturage.Application.Interfaces;
using MediatR;

namespace Covoiturage.Application.Features.Referentiels;

public record GetAllVillesQuery : IRequest<IEnumerable<VilleResponseDto>>;

public class GetAllVillesQueryHandler : IRequestHandler<GetAllVillesQuery, IEnumerable<VilleResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllVillesQueryHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<IEnumerable<VilleResponseDto>> Handle(GetAllVillesQuery request, CancellationToken cancellationToken)
    {
        var villes = await _unitOfWork.Villes.GetAllAsync();
        return villes.OrderBy(v => v.Nom).Select(v => new VilleResponseDto
        {
            Id = v.Id,
            Nom = v.Nom,
            CodePostal = v.CodePostal
        });
    }
}