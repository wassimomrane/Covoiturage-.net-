using Covoiturage.Application.DTOs.Auth;
using MediatR;

namespace Covoiturage.Application.Features.Auth.Commands.VerifyEmail;

public record VerifyEmailCommand(string Email, string Code) : IRequest<AuthResponseDto>;

public class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommand, AuthResponseDto>
{
    private readonly Interfaces.Services.IAuthService _authService;

    public VerifyEmailCommandHandler(Interfaces.Services.IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<AuthResponseDto> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
    {
        return await _authService.VerifyEmailAsync(new VerifyEmailDto
        {
            Email = request.Email,
            Code = request.Code
        });
    }
}