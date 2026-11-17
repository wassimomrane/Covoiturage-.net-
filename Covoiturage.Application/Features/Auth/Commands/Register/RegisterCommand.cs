using Covoiturage.Application.DTOs.Auth;
using MediatR;

namespace Covoiturage.Application.Features.Auth.Commands.Register;

public record RegisterCommand(
    string Nom,
    string Prenom,
    string Email,
    string PhoneNumber,
    string Password
) : IRequest<AuthResponseDto>;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponseDto>
{
    private readonly Interfaces.Services.IAuthService _authService;

    public RegisterCommandHandler(Interfaces.Services.IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<AuthResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        return await _authService.RegisterAsync(new RegisterDto
        {
            Nom = request.Nom,
            Prenom = request.Prenom,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Password = request.Password
        });
    }
}