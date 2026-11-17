using System.Security.Claims;
using Covoiturage.Application.DTOs.Vehicules;
using Covoiturage.Application.Features.Referentiels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Covoiturage.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class VehiculesController : ControllerBase
{
    private readonly IMediator _mediator;

    public VehiculesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("mes-vehicules")]
    public async Task<IActionResult> GetMesVehicules()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        return Ok(await _mediator.Send(new GetMesVehiculesQuery(userId)));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateVehiculeDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var id = await _mediator.Send(new CreateVehiculeCommand(
            userId, dto.Marque, dto.Modele, dto.Immatriculation, dto.NombrePlaces));
        return Ok(new { id, message = "Véhicule ajouté." });
    }
}