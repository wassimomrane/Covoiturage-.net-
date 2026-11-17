using System.Security.Claims;
using Covoiturage.Application.DTOs.Trajets;
using Covoiturage.Application.Features.Trajets.Commands.CreateTrajet;
using Covoiturage.Application.Features.Trajets.Queries.SearchTrajets;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Covoiturage.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TrajetsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TrajetsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("search")]
    [AllowAnonymous]
    public async Task<IActionResult> Search(
        [FromQuery] int? villeDepartId,
        [FromQuery] int? villeArriveeId,
        [FromQuery] DateTime? date,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 6)
    {
        var result = await _mediator.Send(new SearchTrajetsQuery(
            villeDepartId, villeArriveeId, date, page, pageSize));
        return Ok(result);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateTrajetDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var created = await _mediator.Send(new CreateTrajetCommand(
            userId, dto.VilleDepartId, dto.VilleArriveeId, dto.DateDepart, dto.VehiculeId, dto.PlacesDisponibles, dto.PrixParPlace));
        return CreatedAtAction(nameof(Search), new { id = created.Id }, created);
    }
}