using Covoiturage.Domain.Enums;

namespace Covoiturage.Domain.Entities;

public class Reservation
{
    public int Id { get; set; }
    public int NombrePlaces { get; set; }
    public DateTime DateReservation { get; set; } = DateTime.UtcNow;
    public StatutReservation Statut { get; set; } = StatutReservation.EnAttente;

    // FK Trajet
    public int TrajetId { get; set; }
    public Trajet Trajet { get; set; } = null!;

    // FK Passager (ApplicationUser)
    public string PassagerId { get; set; } = string.Empty;
    public ApplicationUser Passager { get; set; } = null!;
    public decimal PrixTotal { get; set; }
}