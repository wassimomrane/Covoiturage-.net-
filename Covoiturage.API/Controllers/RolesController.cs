using Covoiturage.Application.DTOs.Roles;
using Covoiturage.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Covoiturage.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class RolesController : ControllerBase
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public RolesController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetRoles()
    {
        var roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
        return Ok(roles);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto dto)
    {
        if (await _roleManager.RoleExistsAsync(dto.RoleName))
            return BadRequest(new { message = "Ce rôle existe déjà." });

        var result = await _roleManager.CreateAsync(new IdentityRole(dto.RoleName));
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok(new { message = $"Rôle {dto.RoleName} créé avec succès." });
    }

    [HttpPost("assign")]
    public async Task<IActionResult> AssignRole([FromBody] UserRoleDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.UserEmail);
        if (user == null)
            return NotFound(new { message = "Utilisateur introuvable." });

        if (!await _roleManager.RoleExistsAsync(dto.RoleName))
            return NotFound(new { message = "Rôle introuvable." });

        var result = await _userManager.AddToRoleAsync(user, dto.RoleName);
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok(new { message = $"Rôle {dto.RoleName} attribué à {user.Email}." });
    }

    [HttpPost("remove")]
    public async Task<IActionResult> RemoveRole([FromBody] UserRoleDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.UserEmail);
        if (user == null)
            return NotFound(new { message = "Utilisateur introuvable." });

        var result = await _userManager.RemoveFromRoleAsync(user, dto.RoleName);
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok(new { message = $"Rôle {dto.RoleName} retiré à {user.Email}." });
    }
}