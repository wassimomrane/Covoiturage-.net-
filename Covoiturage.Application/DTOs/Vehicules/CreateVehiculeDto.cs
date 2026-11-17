using System.ComponentModel.DataAnnotations;

namespace Covoiturage.Application.DTOs.Vehicules;

public class CreateVehiculeDto
{
    [Required(ErrorMessage = "La marque est obligatoire")]
    [MaxLength(50)]
    public string Marque { get; set; } = string.Empty;

    [Required(ErrorMessage = "Le modèle est obligatoire")]
    [MaxLength(50)]
    public string Modele { get; set; } = string.Empty;

    [Required(ErrorMessage = "L'immatriculation est obligatoire")]
    [MaxLength(30)]
    public string Immatriculation { get; set; } = string.Empty;

    [Range(1, 8, ErrorMessage = "Le nombre de places doit être compris entre 1 et 8")]
    public int NombrePlaces { get; set; }
}