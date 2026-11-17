namespace Covoiturage.Application.DTOs.Auth;

public class AuthResponseDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Token { get; set; }
    public string? Email { get; set; }
    public string? NomComplet { get; set; }
    public IEnumerable<string> Roles { get; set; } = new List<string>();
}