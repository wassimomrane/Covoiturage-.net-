using Covoiturage.Application.DTOs.Auth;

namespace Covoiturage.Application.Interfaces.Services;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
    Task<AuthResponseDto> LoginAsync(LoginDto dto);
    Task<AuthResponseDto> VerifyEmailAsync(VerifyEmailDto dto);
}