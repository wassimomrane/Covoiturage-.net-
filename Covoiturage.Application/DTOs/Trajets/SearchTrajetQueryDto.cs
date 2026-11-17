using System.ComponentModel.DataAnnotations;

namespace Covoiturage.Application.DTOs.Trajets;

public class SearchTrajetQueryDto
{
    public int? VilleDepartId { get; set; }
    public int? VilleArriveeId { get; set; }
    public DateTime? Date { get; set; }

    [Range(1, int.MaxValue)]
    public int Page { get; set; } = 1;

    [Range(1, 50)]
    public int PageSize { get; set; } = 10;
}