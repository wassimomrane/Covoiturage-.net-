using Covoiturage.Application.DTOs.Auth;
using Covoiturage.Application.Features.Auth.Commands.Register;
using Covoiturage.Application.Features.Auth.Commands.VerifyEmail;
using Covoiturage.Application.Features.Auth.Queries.Login;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Covoiturage.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var result = await _mediator.Send(new RegisterCommand(
            dto.Nom, dto.Prenom, dto.Email, dto.PhoneNumber, dto.Password));
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPost("verify-email")]
    public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailDto dto)
    {
        var result = await _mediator.Send(new VerifyEmailCommand(dto.Email, dto.Code));
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var result = await _mediator.Send(new LoginQuery(dto.Email, dto.Password));
        return result.Success ? Ok(result) : Unauthorized(result);
    }
}