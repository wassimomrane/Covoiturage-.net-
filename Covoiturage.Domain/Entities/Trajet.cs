using Covoiturage.Domain.Enums;

namespace Covoiturage.Domain.Entities;

public class Trajet
{
    public int Id { get; set; }
    public DateTime DateDepart { get; set; }
    public int PlacesDisponibles { get; set; }
    public decimal PrixParPlace { get; set; }
    public StatutTrajet Statut { get; set; } = StatutTrajet.Planifie;

    // FK Conducteur
    public string ConducteurId { get; set; } = string.Empty;
    public ApplicationUser Conducteur { get; set; } = null!;

    // FK Véhicule
    public int VehiculeId { get; set; }
    public Vehicule Vehicule { get; set; } = null!;

    // FK Ville de départ
    public int VilleDepartId { get; set; }
    public Ville VilleDepart { get; set; } = null!;

    // FK Ville d'arrivée
    public int VilleArriveeId { get; set; }
    public Ville VilleArrivee { get; set; } = null!;

    // Navigation
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}