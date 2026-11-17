using System.Security.Claims;
using Covoiturage.Application.DTOs.Reservations;
using Covoiturage.Application.Features.Reservations.Commands.CreateReservation;
using Covoiturage.Application.Features.Reservations.Commands.TraiterReservation;
using Covoiturage.Application.Features.Reservations.Queries.GetReservations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Covoiturage.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReservationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReservationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateReservationDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var id = await _mediator.Send(new CreateReservationCommand(userId, dto.TrajetId, dto.NombrePlaces));
        return Ok(new { id, message = "Demande de réservation enregistrée." });
    }

    [HttpGet("mes-reservations")]
    public async Task<IActionResult> GetMesReservations()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        return Ok(await _mediator.Send(new GetMesReservationsQuery(userId)));
    }

    [HttpGet("demandes-conducteur")]
    public async Task<IActionResult> GetDemandesConducteur()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        return Ok(await _mediator.Send(new GetDemandesConducteurQuery(userId)));
    }

    [HttpPatch("{id}/confirmer")]
    public async Task<IActionResult> Confirmer(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var msg = await _mediator.Send(new TraiterReservationCommand(id, userId, ActionReservation.Confirmer));
        return Ok(new { message = msg });
    }

    [HttpPatch("{id}/refuser")]
    public async Task<IActionResult> Refuser(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var msg = await _mediator.Send(new TraiterReservationCommand(id, userId, ActionReservation.Refuser));
        return Ok(new { message = msg });
    }

    [HttpPatch("{id}/annuler")]
    public async Task<IActionResult> Annuler(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var msg = await _mediator.Send(new TraiterReservationCommand(id, userId, ActionReservation.AnnulerPassager));
        return Ok(new { message = msg });
    }

    [HttpPatch("{id}/accepter-annulation")]
    public async Task<IActionResult> AccepterAnnulation(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var msg = await _mediator.Send(new TraiterReservationCommand(id, userId, ActionReservation.AccepterAnnulationConducteur));
        return Ok(new { message = msg });
    }
}