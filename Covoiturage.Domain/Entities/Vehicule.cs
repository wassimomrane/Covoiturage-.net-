using System.Diagnostics;

namespace Covoiturage.Domain.Entities;

public class Vehicule
{
    public int Id { get; set; }
    public string Marque { get; set; } = string.Empty;
    public string Modele { get; set; } = string.Empty;
    public string Immatriculation { get; set; } = string.Empty;
    public int NombrePlaces { get; set; }

    // Clé étrangère Conducteur (ApplicationUser)
    public string ConducteurId { get; set; } = string.Empty;
    public ApplicationUser Conducteur { get; set; } = null!;

    // Navigation
    public ICollection<Trajet> Trajets { get; set; } = new List<Trajet>();
}