using System.ComponentModel.DataAnnotations;

namespace Covoiturage.Application.DTOs.Trajets;

public class CreateTrajetDto
{
    [Required(ErrorMessage = "La date et l'heure de départ sont obligatoires")]
    public DateTime DateDepart { get; set; }

    [Range(1, 8, ErrorMessage = "Le nombre de places doit être compris entre 1 et 8")]
    public int PlacesDisponibles { get; set; }

    [Range(1.0, 500.0, ErrorMessage = "Le prix par place doit être supérieur à 0")]
    public decimal PrixParPlace { get; set; }

    [Required]
    public int VehiculeId { get; set; }

    [Required]
    public int VilleDepartId { get; set; }

    [Required]
    public int VilleArriveeId { get; set; }
}