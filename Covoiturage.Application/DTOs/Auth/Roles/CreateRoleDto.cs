using System.ComponentModel.DataAnnotations;

namespace Covoiturage.Application.DTOs.Roles;

public class CreateRoleDto
{
    [Required]
    public string RoleName { get; set; } = string.Empty;
}