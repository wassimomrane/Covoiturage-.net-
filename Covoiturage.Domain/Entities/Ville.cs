using System.Diagnostics;

namespace Covoiturage.Domain.Entities;

public class Ville
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string CodePostal { get; set; } = string.Empty;

    // Navigation
    public ICollection<Trajet> TrajetsDepart { get; set; } = new List<Trajet>();
    public ICollection<Trajet> TrajetsArrivee { get; set; } = new List<Trajet>();
}