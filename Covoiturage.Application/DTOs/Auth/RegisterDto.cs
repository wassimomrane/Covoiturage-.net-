using System.ComponentModel.DataAnnotations;

namespace Covoiturage.Application.DTOs.Auth;

public class RegisterDto
{
    [Required(ErrorMessage = "Le nom est obligatoire")]
    public string Nom { get; set; } = string.Empty;

    [Required(ErrorMessage = "Le prénom est obligatoire")]
    public string Prenom { get; set; } = string.Empty;

    [Required(ErrorMessage = "L'email est obligatoire")]
    [EmailAddress(ErrorMessage = "Format d'email invalide")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Le numéro de téléphone est obligatoire")]
    [Phone(ErrorMessage = "Format de numéro de téléphone invalide")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Le mot de passe est obligatoire")]
    [MinLength(6, ErrorMessage = "Le mot de passe doit contenir au moins 6 caractères")]
    public string Password { get; set; } = string.Empty;
}