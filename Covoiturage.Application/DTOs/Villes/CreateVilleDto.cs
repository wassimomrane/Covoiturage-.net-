using System.ComponentModel.DataAnnotations;

namespace Covoiturage.Application.DTOs.Villes;

public class CreateVilleDto
{
    [Required(ErrorMessage = "Le nom de la ville est obligatoire")]
    [MaxLength(100)]
    public string Nom { get; set; } = string.Empty;

    [Required(ErrorMessage = "Le code postal est obligatoire")]
    [MaxLength(10)]
    public string CodePostal { get; set; } = string.Empty;
}