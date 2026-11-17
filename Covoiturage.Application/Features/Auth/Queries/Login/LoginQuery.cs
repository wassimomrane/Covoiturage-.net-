using Covoiturage.Application.DTOs.Auth;
using MediatR;

namespace Covoiturage.Application.Features.Auth.Queries.Login;

public record LoginQuery(string Email, string Password) : IRequest<AuthResponseDto>;

public class LoginQueryHandler : IRequestHandler<LoginQuery, AuthResponseDto>
{
    private readonly Interfaces.Services.IAuthService _authService;

    public LoginQueryHandler(Interfaces.Services.IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<AuthResponseDto> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        return await _authService.LoginAsync(new LoginDto
        {
            Email = request.Email,
            Password = request.Password
        });
    }
}