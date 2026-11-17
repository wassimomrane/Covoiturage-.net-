using Covoiturage.Domain.Enums;

namespace Covoiturage.Application.DTOs.Reservations;

public class ReservationResponseDto
{
    public int Id { get; set; }
    public int TrajetId { get; set; }
    public int NombrePlaces { get; set; }
    public DateTime DateReservation { get; set; }
    public StatutReservation Statut { get; set; }

    // Trajet & Villes
    public DateTime DateDepart { get; set; }
    public string VilleDepartNom { get; set; } = string.Empty;
    public string VilleArriveeNom { get; set; } = string.Empty;
    public decimal PrixTotal { get; set; }

    // Passager (pour que le conducteur voie à qui téléphoner)
    public string PassagerId { get; set; } = string.Empty;
    public string PassagerNomComplet { get; set; } = string.Empty;
    public string PassagerPhoneNumber { get; set; } = string.Empty;

    // Conducteur (pour le récapitulatif du passager)
    public string ConducteurNomComplet { get; set; } = string.Empty;
    public string ConducteurPhoneNumber { get; set; } = string.Empty;
}