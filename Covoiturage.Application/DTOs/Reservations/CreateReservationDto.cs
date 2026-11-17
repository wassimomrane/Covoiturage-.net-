using System.ComponentModel.DataAnnotations;

namespace Covoiturage.Application.DTOs.Reservations;

public class CreateReservationDto
{
    [Required]
    public int TrajetId { get; set; }

    [Range(1, 8, ErrorMessage = "Vous devez réserver au moins 1 place.")]
    public int NombrePlaces { get; set; } = 1;
}