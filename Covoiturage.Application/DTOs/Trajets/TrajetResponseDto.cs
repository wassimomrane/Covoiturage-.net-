using Covoiturage.Domain.Enums;

namespace Covoiturage.Application.DTOs.Trajets;

public class TrajetResponseDto
{
    public int Id { get; set; }
    public DateTime DateDepart { get; set; }
    public int PlacesDisponibles { get; set; }
    public decimal PrixParPlace { get; set; }
    public StatutTrajet Statut { get; set; }

    // Infos Départ & Arrivée
    public int VilleDepartId { get; set; }
    public string VilleDepartNom { get; set; } = string.Empty;

    public int VilleArriveeId { get; set; }
    public string VilleArriveeNom { get; set; } = string.Empty;

    // Infos Véhicule
    public int VehiculeId { get; set; }
    public string VehiculeMarque { get; set; } = string.Empty;
    public string VehiculeModele { get; set; } = string.Empty;
    public string VehiculeImmatriculation { get; set; } = string.Empty;

    // Infos Conducteur
    public string ConducteurId { get; set; } = string.Empty;
    public string ConducteurNomComplet { get; set; } = string.Empty;
    public string ConducteurPhoneNumber { get; set; } = string.Empty;
}