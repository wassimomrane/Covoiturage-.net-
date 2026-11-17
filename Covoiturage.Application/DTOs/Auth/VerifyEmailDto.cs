using System.ComponentModel.DataAnnotations;

namespace Covoiturage.Application.DTOs.Auth;

public class VerifyEmailDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(6, MinimumLength = 6, ErrorMessage = "Le code doit comporter exactement 6 chiffres.")]
    public string Code { get; set; } = string.Empty;
}