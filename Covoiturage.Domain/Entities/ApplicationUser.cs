using Microsoft.AspNetCore.Identity;

namespace Covoiturage.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public DateTime DateInscription { get; set; } = DateTime.UtcNow;

    // Champs pour la vérification par code OTP
    public string? VerificationCode { get; set; }
    public DateTime? VerificationCodeExpiresAt { get; set; }

    // Navigation
    public ICollection<Vehicule> Vehicules { get; set; } = new List<Vehicule>();
    public ICollection<Trajet> TrajetsConducteur { get; set; } = new List<Trajet>();
    public ICollection<Reservation> ReservationsPassager { get; set; } = new List<Reservation>();
}