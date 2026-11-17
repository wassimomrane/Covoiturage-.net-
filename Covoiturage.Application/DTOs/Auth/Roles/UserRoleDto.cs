using System.ComponentModel.DataAnnotations;

namespace Covoiturage.Application.DTOs.Roles;

public class UserRoleDto
{
    [Required]
    public string UserEmail { get; set; } = string.Empty;

    [Required]
    public string RoleName { get; set; } = string.Empty;
}