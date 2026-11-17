using System.ComponentModel.DataAnnotations;

namespace Covoiturage.Application.DTOs.Auth;

public class LoginDto
{
    [Required(ErrorMessage = "L'email est obligatoire")]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Le mot de passe est obligatoire")]
    public string Password { get; set; } = string.Empty;
}