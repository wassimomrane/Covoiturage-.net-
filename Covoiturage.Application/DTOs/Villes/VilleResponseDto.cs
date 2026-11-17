namespace Covoiturage.Application.DTOs.Villes;

public class VilleResponseDto
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string CodePostal { get; set; } = string.Empty;
}