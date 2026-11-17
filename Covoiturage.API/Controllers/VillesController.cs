using Covoiturage.Application.Features.Referentiels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Covoiturage.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VillesController : ControllerBase
{
    private readonly IMediator _mediator;

    public VillesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _mediator.Send(new GetAllVillesQuery()));
    }
}