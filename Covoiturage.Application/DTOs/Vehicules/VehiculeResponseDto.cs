namespace Covoiturage.Application.DTOs.Vehicules;

public class VehiculeResponseDto
{
    public int Id { get; set; }
    public string Marque { get; set; } = string.Empty;
    public string Modele { get; set; } = string.Empty;
    public string Immatriculation { get; set; } = string.Empty;
    public int NombrePlaces { get; set; }
    public string ConducteurId { get; set; } = string.Empty;
}